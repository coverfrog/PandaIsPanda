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

        public void OnRoundBegin(RoundData roundData)
        {
            LogUtil.Log($"[{nameof(GameStory)}] 라운드 시작 Id: {roundData.Constant.Id}");
        }
        
        private void OnRoundSec(RoundData roundData)
        {
            LogUtil.Log(roundData.TimerSecInt);
        }
        
        private void OnRoundEnd(RoundData roundData)
        {
            
        }
    }
}