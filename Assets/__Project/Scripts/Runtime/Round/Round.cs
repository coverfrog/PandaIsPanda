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
        public delegate void RoundSecIntHandler(RoundData roundData);
        public delegate void RoundSpawnRequestHandler(SpawnEventData spawnData);
        public delegate void RoundGiveItemRequestHandler(GiveItemEventData giveItemEventData);
        public delegate void RoundEndHandler(RoundData roundData);
        public delegate void RoundLastEndHandler(RoundData roundData);
        
        public event RoundBeginHandler OnRoundBegin;
        public event RoundSecHandler OnRoundSec;
        public event RoundSecIntHandler OnRoundSecInt;
        public event RoundSpawnRequestHandler OnRoundSpawnRequest;
        public event RoundGiveItemRequestHandler OnRoundGiveItemRequest;
        public event RoundEndHandler OnRoundEnd;
        public event RoundLastEndHandler OnRoundLastEnd;

        #endregion

        private IReadOnlyDictionary<ulong, RoundData> m_rounds;
        
        public void Setup
        (
            RoundBeginHandler onRoundBegin,
            RoundSecHandler onRoundSec,
            RoundSecIntHandler onRoundSecInt,
            RoundSpawnRequestHandler onRoundSpawnRequest,
            RoundGiveItemRequestHandler onRoundGiveItemRequest,
            RoundEndHandler onRoundEnd,
            RoundLastEndHandler onRoundLastEnd
        )
        {
            OnRoundBegin -= onRoundBegin;
            OnRoundBegin += onRoundBegin;

            OnRoundSec -= onRoundSec;
            OnRoundSec += onRoundSec;
            
            OnRoundSecInt -= onRoundSecInt;
            OnRoundSecInt += onRoundSecInt;
            
            OnRoundSpawnRequest -= onRoundSpawnRequest;
            OnRoundSpawnRequest += onRoundSpawnRequest;
            
            OnRoundGiveItemRequest -= onRoundGiveItemRequest;
            OnRoundGiveItemRequest += onRoundGiveItemRequest;
            
            OnRoundEnd -= onRoundEnd;
            OnRoundEnd += onRoundEnd;
            
            OnRoundLastEnd -= onRoundLastEnd;
            OnRoundLastEnd += onRoundLastEnd;

            var roundConstants = DataManager.Instance.RoundConstants;
            var spawnEventConstants = DataManager.Instance.SpawnEventConstants;
            var giveItemEventConstants = DataManager.Instance.GiveItemEventConstants;

            m_rounds = roundConstants.ToDictionary(kv => kv.Key, kv =>
                new RoundData(
                    kv.Value,
                    spawnEventConstants,
                    giveItemEventConstants));
        }
        
        public void Play(ulong roundId = 1)
        {
            if (m_rounds.TryGetValue(roundId, out RoundData roundData))
            {
                Invoke_RoundBegin(roundData);
            }
        }
     
        private void Invoke_RoundBegin(RoundData roundData)
        {
            OnRoundBegin?.Invoke(roundData);

            StartCoroutine(CoTimer(roundData));
        }

        private void Invoke_RoundSec(RoundData roundData)
        {
            OnRoundSec?.Invoke(roundData);
        }
        
        private void Invoke_RoundSecInt(RoundData roundData)
        {
            OnRoundSecInt?.Invoke(roundData);
            
            foreach ((_, SpawnEventData spawnEventData) in roundData.SpawnEventData)
            {
                if (spawnEventData.CheckCondition(roundData))
                {
                    Invoke_RoundSpawnRequest(spawnEventData);
                }
            }
            
            foreach ((_, GiveItemEventData giveItemEventData) in roundData.GiveItemEventData)
            {
                if (giveItemEventData.CheckCondition(roundData))
                {
                    Invoke_RoundGiveItemRequest(giveItemEventData);
                }
            }
        }
        
        private void Invoke_RoundSpawnRequest(SpawnEventData spawnEventData)
        {
            OnRoundSpawnRequest?.Invoke(spawnEventData);
        }

        private void Invoke_RoundGiveItemRequest(GiveItemEventData giveItemEventData)
        {
            OnRoundGiveItemRequest?.Invoke(giveItemEventData);
        }
        
        private void Invoke_RoundEnd(RoundData roundData)
        {
            OnRoundEnd?.Invoke(roundData);
        }

        private void Invoke_RoundLastEnd(RoundData roundData)
        {
            OnRoundLastEnd?.Invoke(roundData);
        }
        
        private IEnumerator CoTimer(RoundData roundData)
        {
            Invoke_RoundSecInt(roundData); // 최초의 값은 무조건 동일하므로
            Invoke_RoundSec(roundData);
            
            for (float timerSec = roundData.Constant.Duration; timerSec > 0.0f; timerSec -= Time.deltaTime)
            {
                roundData.SetTimerSec(timerSec);
                Invoke_RoundSec(roundData);
                
                int secInt = Mathf.CeilToInt(timerSec);
                if (roundData.TimerSecInt != secInt)
                {
                    roundData.SetTimerSecInt(Mathf.CeilToInt(timerSec));
                    Invoke_RoundSecInt(roundData);
                }
                
                yield return null;
            }
            
            Invoke_RoundEnd(roundData);

            if (m_rounds.TryGetValue(roundData.Constant.NextId, out RoundData nextRoundData))
            {
                Invoke_RoundBegin(nextRoundData);
            }
            else
            {
                Invoke_RoundLastEnd(roundData);
            }
        }
    }
}