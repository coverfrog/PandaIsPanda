using System;
using System.Collections.Generic;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class RoundConstant
    {
        [SerializeField] private ulong m_id;
        [SerializeField] private ulong m_nextId;
        [SerializeField] private List<ulong> m_spawnEventIds;
        [SerializeField] private float m_duration;
        
        public ulong Id => m_id;
        public ulong NextId => m_nextId;
        public IReadOnlyList<ulong> SpawnEventIds => m_spawnEventIds;
        public float Duration => m_duration;

        public RoundConstant(ulong id, ulong nextId, List<ulong> spawnEventIds, float duration)
        {
            m_id = id;
            m_nextId = nextId;
            m_spawnEventIds = spawnEventIds;
            m_duration = duration;
        }
    }
}