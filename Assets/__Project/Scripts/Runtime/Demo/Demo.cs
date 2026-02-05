using System;
using UnityEngine;

namespace PandaIsPanda
{
    public class Demo : MonoBehaviour
    {
        private Board m_board;

        private void Awake()
        {
            m_board = FindAnyObjectByType<Board>();
        }

        private void OnEnable()
        {
            m_board.Init();
        }

        private void OnDisable()
        {
            
        }
    }
}

