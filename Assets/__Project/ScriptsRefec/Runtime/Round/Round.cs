using System;
using UnityEngine;
using UnityEngine.Events;

namespace PandaIsPanda
{
    public class Round : MonoBehaviour
    {
        #region # Event

        public delegate void RoundBeginHandler(ulong roundId);
        
        public delegate void RoundSecHandler(ulong roundId, uint sec);
        
        public delegate void RoundEndHandler(ulong roundId);
        
        public event RoundBeginHandler OnRoundBegin;
        
        public event RoundSecHandler OnRoundSec;
        
        public event RoundEndHandler OnRoundEnd;

        #endregion

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
        }

        public void Play()
        {
            OnRoundBegin?.Invoke(0);
        }
    }
}