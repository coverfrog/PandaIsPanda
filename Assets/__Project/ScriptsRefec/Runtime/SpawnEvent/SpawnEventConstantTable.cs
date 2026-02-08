using System;
using System.Collections.Generic;
using UnityEngine;

namespace PandaIsPanda
{
    public class SpawnEventConstantTable : ScriptableObject, IConstantTable
    {
        [SerializeField] private UnityDictionary<ulong, SpawnEventConstant> m_data = new();
        
        public void Load(IReadOnlyDictionary<int, IReadOnlyDictionary<int, IReadOnlyList<object>>> data)
        {
            m_data.Clear();

            foreach ((int row, IReadOnlyList<object> columns) in data[0])
            {
                if (row < 2)
                    continue;
                
                ulong id = Convert.ToUInt64(columns[0]);
                
                var constant = new SpawnEventConstant(id);
                
                m_data.Add(id, constant);
            }
        }
    }
}