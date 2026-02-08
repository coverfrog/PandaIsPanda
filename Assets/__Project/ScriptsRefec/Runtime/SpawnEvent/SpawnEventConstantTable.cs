using System;
using System.Collections.Generic;
using UnityEngine;

namespace PandaIsPanda
{
    public class SpawnEventConstantTable : ScriptableObject, IConstantTable
    {
        [SerializeField] private UnityDictionary<ulong, SpawnEventConstant> m_data = new();

        public IReadOnlyDictionary<ulong, SpawnEventConstant> Data 
        {
            get
            {
                if (m_readOnlyData == null)
                    m_readOnlyData = m_data.ToReadOnlyDictionary();

                return m_readOnlyData;
            }
        }
        
        private IReadOnlyDictionary<ulong, SpawnEventConstant> m_readOnlyData;
        
        public void Load(IReadOnlyDictionary<int, IReadOnlyDictionary<int, IReadOnlyList<object>>> data)
        {
            m_data.Clear();

            foreach ((int row, IReadOnlyList<object> columns) in data[0])
            {
                if (row < 2)
                    continue;
                
                ulong id = Convert.ToUInt64(columns[0]);
                ulong unitId = Convert.ToUInt64(columns[1]);
                SpawnEventTriggerType spawnEventTriggerType = (SpawnEventTriggerType)Convert.ToInt32(columns[2]);
                int callCount = Convert.ToInt32(columns[3]);
                int spawnCount = Convert.ToInt32(columns[4]);
                
                var constant = new SpawnEventConstant(id, unitId, spawnEventTriggerType, callCount, spawnCount);
                
                m_data.Add(id, constant);
            }
        }
    }
}