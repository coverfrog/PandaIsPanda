using System;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class RoundConstant
    {
        [SerializeField] private ulong m_id;
        [SerializeField] private ulong m_nextId;
        [SerializeField] private float m_duration;
        
        public ulong Id => m_id;
        public ulong NextId => m_nextId;
        public float Duration => m_duration;

        public RoundConstant(ulong id, ulong nextId, float duration)
        {
            m_id = id;
            m_nextId = nextId;
            m_duration = duration;
        }
    }
}