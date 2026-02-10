using System;
using RTLTMPro;
using UnityEngine;

namespace PandaIsPanda
{
    public class UIPageGameStory : UIPage
    {
        [Header("# References")]
        [SerializeField] private RTLTextMeshPro m_txtRound;
        [SerializeField] private RTLTextMeshPro m_txtTimer;
        [SerializeField] private RTLTextMeshPro m_txtUnitCount;
        [SerializeField] private RTLTextMeshPro m_txtGold;
        [SerializeField] private RTLTextMeshPro m_txtBamboo;
        
        public void Open(ulong sessionId)
        {
            var data = DataManager.Instance.GameStoryData[sessionId];
            
            data.EnemyCount.OnValueChanged -= OnEnemyCountChanged;
            data.EnemyCount.OnValueChanged += OnEnemyCountChanged;
            
            OnEnemyCountChanged(data.EnemyCount.Value);
            
            data.Round.OnValueChanged -= OnRoundChanged;
            data.Round.OnValueChanged += OnRoundChanged;

            OnRoundChanged(data.Round.Value);
            
            data.Timer.OnValueChanged -= OnTimerChanged;
            data.Timer.OnValueChanged += OnTimerChanged;
            
            OnTimerChanged(data.Timer.Value);
            
        }

        private void OnTimerChanged(float value)
        {
            if (!gameObject.activeInHierarchy ||
                !m_txtTimer)
                return;
            
            m_txtTimer.text = $"Timer : {value:0.0}";
        }

        private void OnRoundChanged(ulong value)
        {
            if (!gameObject.activeInHierarchy ||
                !m_txtRound)
                return;

            m_txtRound.text = $"Round : {value}";
        }

        private void OnEnemyCountChanged(int value)
        {
            if (!gameObject.activeInHierarchy ||
                !m_txtUnitCount)
                return;

            m_txtUnitCount.text = $"Unit Count: {value}";
        }
    }
}