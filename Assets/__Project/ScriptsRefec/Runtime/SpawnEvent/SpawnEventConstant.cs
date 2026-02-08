using System;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class SpawnEventConstant
    {
        [SerializeField] private ulong m_id;
        [SerializeField] private SpawnEventTriggerType m_spawnEventTriggerType;
        [SerializeField] private ulong m_unitId;
        [SerializeField] private int m_callCount;
        [SerializeField] private int m_spawnCount;

        public ulong Id => m_id;

        public SpawnEventTriggerType SpawnEventTriggerType => m_spawnEventTriggerType;
        
        public ulong UnitId => m_unitId;

        public int CallCount => m_callCount;
        public int SpawnCount => m_spawnCount;

        public SpawnEventConstant
        (
            ulong id,
            SpawnEventTriggerType spawnEventTriggerType,
            ulong unitId,
            int callCount,
            int spawnCount
        )
        {
            m_id = id;
            m_spawnEventTriggerType = spawnEventTriggerType;
            m_unitId = unitId;
            m_callCount = callCount;
            m_spawnCount = spawnCount;
        }
    }
}