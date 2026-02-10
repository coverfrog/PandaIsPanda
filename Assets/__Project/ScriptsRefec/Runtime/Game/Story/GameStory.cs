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
        [SerializeField] private PointCircleGroup m_pointCircleGroup;
        
        private IObjectPool<Unit> m_enemyPool;

        private ulong m_sessionId;

        private void OnDestroy()
        {
            m_enemyPool?.Clear();
        }

        public void Setup()
        {
            DataManager.Instance.GameStoryData.TryAdd(m_sessionId, new GameStoryData());
            
            m_enemyPool = new ObjectPool<Unit>(OnEnemyCreate, OnEnemyGet, OnEnemyRelease, OnEnemyDestroy);
            
            m_round.Setup
            (
                OnRoundBegin,
                OnRoundSec,
                OnRoundSecInt,
                OnRoundSpawnRequest,
                OnRoundEnd,
                OnRoundLastEnd
            );
            
            Play();
        }

      

        public void Play()
        {
            m_round.Play();

            UIPageGameStory ui = UIManager.Instance.GetPage<UIPageGameStory>(UIPageType.GameStory);
            ui.Open(m_sessionId);
        }

        #region # On Round

        public void OnRoundBegin(RoundData roundData)
        {
            LogUtil.Log($"[{nameof(GameStory)}] 라운드 시작 Id: {roundData.Constant.Id}");
            
            DataManager.Instance.GameStoryData[m_sessionId].Round.Value = roundData.Constant.Id;
        }
        
        private void OnRoundSec(RoundData roundData)
        {
            DataManager.Instance.GameStoryData[m_sessionId].Timer.Value = roundData.TimerSec;
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
            
            pointFollower.Setup(m_pointCircleGroup.Points);
            
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
    }
}