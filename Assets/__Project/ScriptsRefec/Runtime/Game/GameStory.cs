using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PandaIsPanda
{
    public class GameStory : MonoBehaviour, IGame
    {
        [Header("# References")]
        [SerializeField] private Round m_round;
        
        public void Setup()
        {
            m_round.Setup
            (
                OnRoundBegin,
                OnRoundSec,
                OnRoundEnd
            );
            
            Play();
        }

        public void Play()
        {
            m_round.Play();
        }

        public void OnRoundBegin(ulong roundId)
        {
            LogUtil.Log($"[{nameof(GameStory)}] 라운드 시작 Id: {roundId}");
        }
        
        private void OnRoundSec(ulong roundId, uint sec)
        {
            
        }
        
        private void OnRoundEnd(ulong roundId)
        {
            
        }
    }
}