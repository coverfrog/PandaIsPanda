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
        
        public delegate void RoundEndHandler(RoundData roundData);
        
        public event RoundBeginHandler OnRoundBegin;
        
        public event RoundSecHandler OnRoundSec;
        
        public event RoundEndHandler OnRoundEnd;

        #endregion

        private IReadOnlyDictionary<ulong, RoundData> m_rounds;
        
        public void Setup
        (
            RoundBeginHandler onRoundBegin,
            RoundSecHandler onRoundSec,
            RoundEndHandler onRoundEnd
        )
        {
            OnRoundBegin -= onRoundBegin;
            OnRoundBegin += onRoundBegin;
            
            OnRoundSec -=  onRoundSec;
            OnRoundSec +=  onRoundSec;
            
            OnRoundEnd -= onRoundEnd;
            OnRoundEnd += onRoundEnd;

            IReadOnlyDictionary<ulong, RoundConstant> constants = AddressableUtil.Load<RoundConstantTable>("constanttable/round").Data;
            m_rounds = constants.ToDictionary(kv => kv.Key, kv => new RoundData(kv.Value));
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
        }
        
        private void InvokeRoundEnd(RoundData roundData)
        {
            OnRoundEnd?.Invoke(roundData);
        }
        
        private IEnumerator CoTimer(RoundData roundData)
        {
            InvokeRoundSec(roundData); // 최초의 값은 무조건 동일하므로
            
            for (float timerSec = roundData.Constant.Duration; timerSec > 0.0f; timerSec -= Time.deltaTime)
            {
                int sec = Mathf.CeilToInt(timerSec);
                
                if (roundData.TimerSec != sec)
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
        }
    }
}