using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PandaIsPandaMvp
{
    public class Round : MonoBehaviour
    {
        [Header("# References")] 
        [SerializeField] private UIRound m_uiRound;
        
        [Header("# Data")]
        [SerializeField] private RoundData m_roundData;

        [Header("# Res")]
        [SerializeField] private RoundConstantTable m_resConstantTable;
        
        public void Setup()
        {
            
        }

        public void Begin()
        {
            StartCoroutine(CoTimer());
        }

        private IEnumerator CoTimer()
        {
            // 매직 넘버
            for (int i = 0; i < 2; i++)
            {
                bool isBoss = i == 4;
                
                for (float t = 10.0f; t >= 0.0f; t -= Time.deltaTime)
                {
                    float min = t / 60.0f;
                    float sec = t % 60.0f;
                
                    m_uiRound.SetTimer(min, sec);
                
                    yield return null;
                }
            }
            
            //
            bool isWin = FindAnyObjectByType<Monster>();
            Debug.Log(isWin ? "승리" : "패배");
        }
    }
}