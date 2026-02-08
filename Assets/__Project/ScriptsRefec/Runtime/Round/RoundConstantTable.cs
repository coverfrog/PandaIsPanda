using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PandaIsPanda
{
    public class RoundConstantTable : ScriptableObject, IConstantTable
    {
        [SerializeField] private UnityDictionary<ulong, RoundConstant> m_data = new();

        public IReadOnlyDictionary<ulong, RoundConstant> Data 
        {
            get
            {
                if (m_readOnlyData == null)
                    m_readOnlyData = m_data.ToReadOnlyDictionary();

                return m_readOnlyData;
            }
        }
        
        private IReadOnlyDictionary<ulong, RoundConstant> m_readOnlyData;
        
        public void Load(IReadOnlyDictionary<int, IReadOnlyDictionary<int, IReadOnlyList<object>>> data)
        {
            m_data.Clear();

            foreach ((int row, IReadOnlyList<object> columns) in data[0])
            {
                if (row < 2)
                    continue;
                
                ulong id = Convert.ToUInt64(columns[0]);
                ulong nextId = Convert.ToUInt64(columns[1]);
                List<ulong> spawnIds = Convert.ToString(columns[2]).Split('_').Select(s =>Convert.ToUInt64(s)).ToList();
                float duration = Convert.ToSingle(columns[3]);
                
                var constant = new RoundConstant(id, nextId, spawnIds, duration);
                
                m_data.Add(id, constant);
            }
        }
    }
}