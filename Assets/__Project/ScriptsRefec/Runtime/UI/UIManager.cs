using System;
using UnityEngine;

namespace PandaIsPanda
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}