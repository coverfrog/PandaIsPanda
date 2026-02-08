using System;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class RoundConstant
    {
        [SerializeField] private ulong m_id;
        [SerializeField] private float m_duration;
        
        public ulong Id => m_id;
        public float Duration => m_duration;
    }
}