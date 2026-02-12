using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using Random = System.Random;

namespace PandaIsPanda
{
    public class GameStory : MonoBehaviour, IGame
    {
        [Header("# References")]
        [SerializeField] private Round m_round;
        [SerializeField] private PointCircleGroup m_pointsEnemy;
        [SerializeField] private PointCircleGroup m_pointsAlias;
        
        private IObjectPool<Unit> m_enemyPool;
        private IObjectPool<Unit> m_aliasPool;

        private ulong m_sessionId;
        
        private void OnDestroy()
        {
            m_enemyPool?.Clear();
        }

        public void Setup()
        {
            if (!DataManager.Instance.GameStoryData.TryAdd(m_sessionId, new GameStoryData()))
                DataManager.Instance.GameStoryData[m_sessionId] = new GameStoryData();
            
            GameStoryData data = DataManager.Instance.GameStoryData[m_sessionId];

            m_pointsAlias.SetEdgeCount(data.AliasMaxCount.Value).Spread();
            
            m_enemyPool = new ObjectPool<Unit>(OnEnemyCreate, OnEnemyGet, OnEnemyRelease, OnEnemyDestroy);
            m_aliasPool = new ObjectPool<Unit>(OnAliasCreate, OnAliasGet, OnAliasRelease, OnAliasDestroy);
            
            m_round.Setup
            (
                OnRoundBegin,
                OnRoundSec,
                OnRoundSecInt,
                OnRoundSpawnRequest,
                OnRoundGiveItemRequest, 
                OnRoundEnd,
                OnRoundLastEnd
            );

            var ui = UIManager.Instance.GetPage<UIPageGameStory>(UIPageType.GameStory);
            ui.Open
            (
                data,
                OnUIGachaRequest
            );
            
            Play();
        }

        public void Play()
        {
            m_round.Play();
        }

        #region # OnRound

        public void OnRoundBegin(RoundData roundData)
        {
            LogUtil.Log($"[{nameof(GameStory)}] 라운드 시작 Id: {roundData.Constant.Id}");
            
            var data = DataManager.Instance.GameStoryData[m_sessionId];
            data.Round.Value = roundData.Constant.Id;
        }
        
        private void OnRoundSec(RoundData roundData)
        {
            var data = DataManager.Instance.GameStoryData[m_sessionId];
            data.Timer.Value = roundData.TimerSec;
        }
        
        private void OnRoundSecInt(RoundData roundData)
        {
            // LogUtil.Log($"[{nameof(GameStory)}] 라운드 남은 시간 : {roundData.TimerSecInt}");
        }
        
        private void OnRoundSpawnRequest(SpawnEventData spawnEventData)
        {
            int spawnCount = spawnEventData.Constant.SpawnCount;
            ulong spawnId = spawnEventData.Constant.UnitId;
            
            for (int i = 0; i < spawnCount; i++)
            {
                var unit = m_enemyPool.Get();
                var unitConstant = DataManager.Instance.UnitConstants[spawnId];
                var unitData = new UnitData(unitConstant);                
                
                unit.Setup
                (
                    unitData
                );

                if (unit.gameObject.TryGetComponent(out PointFollower pointFollower))
                {
                    pointFollower.Follow();
                }
            }

            spawnEventData.AddCallCount();
            
            // LogUtil.Log($"[{nameof(GameStory)}] 소환");
        }
        
        private void OnRoundGiveItemRequest(GiveItemEventData giveItemEventData)
        {
            var data = DataManager.Instance.GameStoryData[m_sessionId];
            var inventory = data.InventoryData;
            
            foreach (CountValue<ulong> itemCv in giveItemEventData.Constant.GiveItems)
            {
                ulong id = itemCv.value;
                int count = itemCv.count;
                
                inventory.AddItem(id.ToItemData(count));
            }

            giveItemEventData.Call();
        }
        
        private void OnRoundEnd(RoundData roundData)
        {
            
        }
        
        private void OnRoundLastEnd(RoundData roundData)
        {
            LogUtil.Log($"[{nameof(GameStory)}] 마지막 라운드 종료");
        }
        #endregion

        #region # OnEnemy

        private Unit OnEnemyCreate()
        {
            var unit = AddressableUtil.Instantiate<Unit>("unit/unit", false);
            var pointFollower = unit.gameObject.AddComponent<PointFollower>();
            
            pointFollower.Setup(m_pointsEnemy.Points);
            
            return unit;
        }
        
        private void OnEnemyGet(Unit unit)
        {
            unit.gameObject.SetActive(true);

            DataManager.Instance.GameStoryData[m_sessionId].EnemyCount.Value++;
        }

        private void OnEnemyRelease(Unit unit)
        {
            unit.gameObject.SetActive(false);
            
            DataManager.Instance.GameStoryData[m_sessionId].EnemyCount.Value--;
        }
        
        private void OnEnemyDestroy(Unit unit)
        {
            Destroy(unit.gameObject);
        }

        #endregion

        #region # OnAlias

        private Unit OnAliasCreate()
        {
            var unit = AddressableUtil.Instantiate<Unit>("unit/unit", false);
            
            return unit;
        }
        
        private void OnAliasGet(Unit unit)
        {
            unit.gameObject.SetActive(true);
            
            DataManager.Instance.GameStoryData[m_sessionId].AliasCount.Value++;
            
        }

        private void OnAliasRelease(Unit unit)
        {
            unit.gameObject.SetActive(false);
            
            DataManager.Instance.GameStoryData[m_sessionId].AliasCount.Value--;
        }
        
        private void OnAliasDestroy(Unit unit)
        {
            Destroy(unit.gameObject);
        }

        #endregion

        #region # OnUI

        private void OnUIGachaRequest(ulong costId)
        {
            IReadOnlyDictionary<ulong, GachaConstant> gachaConstants = costId switch
            {
                GachaCostKey.k_roundNormal => DataManager.Instance.RoundGachaNormalConstants,
                GachaCostKey.k_roundUnique => DataManager.Instance.RoundGachaUniqueConstants,
                _ => null
            };
            

            if (gachaConstants == null)
                return;

            double total = gachaConstants.Values.Sum(c => c.Probability);
            double rand = new Random().NextDouble() * total;

            double cumulative = 0.0f;
            GachaConstant gachaConstantSelect = null;
            
            foreach (GachaConstant gachaConstant in gachaConstants.Values)
            {
                cumulative += gachaConstant.Probability;
                if (rand > cumulative)
                {
                    continue;
                }
                
                gachaConstantSelect = gachaConstant;
                break;
            }

            if (gachaConstantSelect == null)
                return;
            
            var costItems = DataManager.Instance.GachaCostConstants[costId].CostItems;
            var unitId = gachaConstantSelect.UnitId;
            var data = DataManager.Instance.GameStoryData[m_sessionId];
            var inventory = data.InventoryData;
            
            foreach (CountValue<ulong> cv in costItems)
            {
                inventory.RemoveItem(cv);
            }

            var unitConstant = DataManager.Instance.UnitConstants[unitId];
            var unitData = new UnitData(unitConstant);
            var unit = m_aliasPool.Get();

            var position = m_pointsAlias.Points[data.AliasCount.Value - 1];
            
            unit.Setup(unitData).SetPosition(position);
            
            LogUtil.Log($"[{nameof(GameStory)}] 가챠 요청 - {costId} - {position}");
        }

        #endregion
    }
}