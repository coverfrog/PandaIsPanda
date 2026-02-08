using System;
using System.Collections.Generic;
using UnityEngine;

namespace PandaIsPanda
{
    public class RoundConstantTable : ScriptableObject, IConstantTable
    {
        [SerializeField] private UnityDictionary<ulong, RoundConstant> m_data = new();

        public IReadOnlyDictionary<ulong, RoundConstant> Data => m_data.ToReadOnlyDictionary();
        
        public void Load(IReadOnlyDictionary<int, IReadOnlyDictionary<int, IReadOnlyList<object>>> data)
        {
            m_data.Clear();

            foreach ((int row, IReadOnlyList<object> columns) in data[0])
            {
                if (row < 2)
                    continue;
                
                ulong id = Convert.ToUInt64(columns[0]);
                ulong nextId = Convert.ToUInt64(columns[1]);
                float duration = Convert.ToSingle(columns[2]);
                
                var constant = new RoundConstant(id, nextId,duration);
                
                m_data.Add(id, constant);
            }
        }
    }
}