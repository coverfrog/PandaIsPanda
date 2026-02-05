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

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                m_board.AddItem();
            }
#endif
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

