using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;

namespace PandaIsPanda
{
    public class GameStory : MonoBehaviour, IGame
    {
        [Header("# References")]
        [SerializeField] private Round m_round;
        [SerializeField] private Inventory m_inventory;
        
        private IObjectPool<Unit> m_enemyPool;

        private void OnDestroy()
        {
            if (m_enemyPool != null) m_enemyPool.Clear();
        }

        public void Setup()
        {
            m_enemyPool = new ObjectPool<Unit>(OnUnitCreate, OnUnitGet, OnUnitRelease, OnUnitDestroy);
            
            m_round.Setup
            (
                OnRoundBegin,
                OnRoundSec,
                OnRoundSpawnRequest,
                OnRoundEnd
            );
            
            Play();
        }

        public void Play()
        {
            m_round.Play();
        }

        #region # On Round

        public void OnRoundBegin(RoundData roundData)
        {
            LogUtil.Log($"[{nameof(GameStory)}] 라운드 시작 Id: {roundData.Constant.Id}");
        }
        
        private void OnRoundSec(RoundData roundData)
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
                
                unit.Setup(unitData);
            }

            spawnEventData.AddCallCount();
            
            LogUtil.Log($"[{nameof(GameStory)}] 소환");
        }
        
        private void OnRoundEnd(RoundData roundData)
        {
            
        }
        #endregion

        #region # On UnitSpawn

        private Unit OnUnitCreate()
        {
            return AddressableUtil.Instantiate<Unit>("unit/unit", false);
        }
        
        private void OnUnitGet(Unit unit)
        {
            
        }

        private void OnUnitRelease(Unit unit)
        {
            
        }

        
        private void OnUnitDestroy(Unit unit)
        {
            
        }

        #endregion
    }
}