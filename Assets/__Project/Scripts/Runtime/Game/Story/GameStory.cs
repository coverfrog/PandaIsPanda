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
        
        private IObjectPool<Unit> m_unitPool;

        private GameStoryData m_data;

        private void OnDestroy()
        {
            m_unitPool?.Clear();
        }

        public void Setup()
        {
            const int sessionId = 0;
            
            if (!DataManager.Instance.GameStoryData.TryAdd(sessionId, new GameStoryData()))
                DataManager.Instance.GameStoryData[sessionId] = new GameStoryData();
            
            m_data = DataManager.Instance.GameStoryData[sessionId];

            m_pointsAlias.SetEdgeCount(m_data.AliasMaxCount.Value).Spread();
            
            m_unitPool = new ObjectPool<Unit>(OnUnitCreate, OnUnitGet, OnUnitRelease, OnUnitDestroy);
            
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
                m_data,
                OnGachaRequest,
                OnSelectUnit
            );
            
            Play();
        }

        public void Play()
        {
            m_round.Play();
        }

        private void Spawn(ulong unitId, UnitCtrlType unitCtrlType)
        {
            var constant = DataManager.Instance.UnitConstants[unitId];
            var data = new UnitData(constant, unitCtrlType);
            var unit = m_unitPool.Get().SetData(data).SetPool(m_unitPool).SetEvents(OnUnitIsLive, OnUnitHp);
            
            if (unitCtrlType == UnitCtrlType.Enemy)
            {
                var pointFollower = unit.gameObject.AddComponent<PointFollower>();
                pointFollower.SetPoints(m_pointsEnemy.Points).Follow();

                m_data.EnemyCount.Value++;
            }

            else
            {
                var unitAutoAttack = unit.gameObject.AddComponent<UnitAutoAttack>();
                unitAutoAttack.SetUnit(unit).AutoAttack();
                
                unit.SetPosition(m_pointsAlias.Points[m_data.AliasCount.Value]);

                m_data.AliasCount.Value++;
            }
        }

    
        private ulong GetUnitId(ulong costId)
        {
            IReadOnlyDictionary<ulong, GachaConstant> gachaConstants = costId switch
            {
                GachaCostKey.k_roundNormal => DataManager.Instance.RoundGachaNormalConstants,
                GachaCostKey.k_roundUnique => DataManager.Instance.RoundGachaUniqueConstants,
                _ => null
            };

            double total = gachaConstants!.Values.Sum(c => c.Probability);
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

            return gachaConstantSelect!.UnitId;
        }

        private void ConsumeItems(ulong costId)
        {
            var costItems = DataManager.Instance.GachaCostConstants[costId].CostItems;
            var inventory = m_data.InventoryData;
            
            foreach (CountValue<ulong> cv in costItems)
            {
                inventory.RemoveItem(cv);
            }
        }

        #region # OnUI
        
        private void OnGachaRequest(ulong costId)
        {
            ulong unitId = GetUnitId(costId);
            ConsumeItems(costId);
            Spawn(unitId, UnitCtrlType.Alias);
        }
        
        private void OnSelectUnit(Unit unit)
        {
            m_data.SelectedUnit.Value = unit;
        }

        #endregion

        #region # OnRound

        public void OnRoundBegin(RoundData roundData)
        {
            LogUtil.Log($"[{nameof(GameStory)}] 라운드 시작 Id: {roundData.Constant.Id}");
            
            m_data.Round.Value = roundData.Constant.Id;
        }
        
        private void OnRoundSec(RoundData roundData)
        {
            m_data.Timer.Value = roundData.TimerSec;
        }
        
        private void OnRoundSecInt(RoundData roundData)
        {
            
        }
        
        private void OnRoundSpawnRequest(SpawnEventData spawnEventData)
        {
            int spawnCount = spawnEventData.Constant.SpawnCount;
            ulong spawnId = spawnEventData.Constant.UnitId;
            
            for (int i = 0; i < spawnCount; i++)
            {
                Spawn(spawnId, UnitCtrlType.Enemy);
            }

            spawnEventData.AddCallCount();
            
            // LogUtil.Log($"[{nameof(GameStory)}] 소환");
        }
        
        private void OnRoundGiveItemRequest(GiveItemEventData giveItemEventData)
        {
            var inventory = m_data.InventoryData;
            
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

        #region # OnUnit

        private Unit OnUnitCreate()
        {
            var unit = AddressableUtil.Instantiate<Unit>("unit/unit", false);
            
            return unit;
        }
        
        private void OnUnitGet(Unit unit)
        {
            if (!unit) return;
            
            unit.gameObject.SetActive(true);
        }

        private void OnUnitRelease(Unit unit)
        {
            if (!unit) return;
            
            unit.gameObject.SetActive(false);
        }
        
        private void OnUnitDestroy(Unit unit)
        {
            if (!unit) return;
            
            Destroy(unit.gameObject);
        }

        #endregion

        #region # OnUnit

        private void OnUnitHp(Unit sender, Unit unit, float hp)
        {
            
        }

        private void OnUnitIsLive(Unit sender, Unit unit, bool isLive)
        {
            
        }

        #endregion
    }
}