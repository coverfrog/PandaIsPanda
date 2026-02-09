using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PandaIsPanda
{
    public class Round : MonoBehaviour
    {
        #region # Event

        public delegate void RoundBeginHandler(RoundData roundData);
        public delegate void RoundSecHandler(RoundData roundData);
        public delegate void RoundSpawnRequestHandler(SpawnEventData spawnData);
        public delegate void RoundEndHandler(RoundData roundData);
        public delegate void RoundLastEndHandler(RoundData roundData);
        
        public event RoundBeginHandler OnRoundBegin;
        public event RoundSecHandler OnRoundSec;
        public event RoundSpawnRequestHandler OnRoundSpawnRequest;
        public event RoundEndHandler OnRoundEnd;
        public event RoundLastEndHandler OnRoundLastEnd;

        #endregion

        private IReadOnlyDictionary<ulong, RoundData> m_rounds;
        
        public void Setup
        (
            RoundBeginHandler onRoundBegin,
            RoundSecHandler onRoundSec,
            RoundSpawnRequestHandler onRoundSpawnRequest,
            RoundEndHandler onRoundEnd,
            RoundLastEndHandler onRoundLastEnd
        )
        {
            OnRoundBegin -= onRoundBegin;
            OnRoundBegin += onRoundBegin;
            
            OnRoundSec -= onRoundSec;
            OnRoundSec += onRoundSec;
            
            OnRoundSpawnRequest -= onRoundSpawnRequest;
            OnRoundSpawnRequest += onRoundSpawnRequest;
            
            OnRoundEnd -= onRoundEnd;
            OnRoundEnd += onRoundEnd;
            
            OnRoundLastEnd -= onRoundLastEnd;
            OnRoundLastEnd += onRoundLastEnd;

            var roundConstants = DataManager.Instance.RoundConstants;
            var spawnEventConstants = DataManager.Instance.SpawnEventConstants;

            m_rounds = roundConstants.ToDictionary(kv => kv.Key, kv =>
                new RoundData(
                    kv.Value,
                    spawnEventConstants));
        }
        
        public void Play(ulong roundId = 1)
        {
            if (m_rounds.TryGetValue(roundId, out RoundData roundData))
            {
                InvokeRoundBegin(roundData);
            }
        }
     
        private void InvokeRoundBegin(RoundData roundData)
        {
            OnRoundBegin?.Invoke(roundData);

            StartCoroutine(CoTimer(roundData));
        }

        private void InvokeRoundSec(RoundData roundData)
        {
            OnRoundSec?.Invoke(roundData);
            
            foreach ((_, SpawnEventData spawnEventData) in roundData.SpawnEventData)
            {
                if (spawnEventData.CheckCondition(roundData))
                {
                    InvokeRoundSpawnRequest(spawnEventData);
                }
            }
        }
        
        private void InvokeRoundSpawnRequest(SpawnEventData spawnEventData)
        {
            OnRoundSpawnRequest?.Invoke(spawnEventData);
        }
        
        private void InvokeRoundEnd(RoundData roundData)
        {
            OnRoundEnd?.Invoke(roundData);
        }

        private void InvokeRoundLastEnd(RoundData roundData)
        {
            OnRoundLastEnd?.Invoke(roundData);
        }
        
        private IEnumerator CoTimer(RoundData roundData)
        {
            InvokeRoundSec(roundData); // 최초의 값은 무조건 동일하므로
            
            for (float timerSec = roundData.Constant.Duration; timerSec > 0.0f; timerSec -= Time.deltaTime)
            {
                int sec = Mathf.CeilToInt(timerSec);
                
                if (roundData.TimerSecInt != sec)
                {
                    roundData.SetTimerSec(Mathf.CeilToInt(timerSec));
                    
                    InvokeRoundSec(roundData);
                }
                
                yield return null;
            }
            
            InvokeRoundEnd(roundData);

            if (m_rounds.TryGetValue(roundData.Constant.NextId, out RoundData nextRoundData))
            {
                InvokeRoundBegin(nextRoundData);
            }
            else
            {
                InvokeRoundLastEnd(roundData);
            }
        }
    }
}