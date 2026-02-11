using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PandaIsPanda
{
    public class GachaConstantTable : ScriptableObject, IConstantTable
    {
        [SerializeField] private UnityDictionary<ulong, GachaConstant> m_data = new();
        
        public IReadOnlyDictionary<ulong, GachaConstant> Data 
        {
            get
            {
                if (m_readOnlyData == null)
                    m_readOnlyData = m_data.ToReadOnlyDictionary();

                return m_readOnlyData;
            }
        }
        
        private IReadOnlyDictionary<ulong, GachaConstant> m_readOnlyData;
        
        
        public void Load(IReadOnlyDictionary<int, IReadOnlyDictionary<int, IReadOnlyList<object>>> data)
        {
            m_data.Clear();

            foreach ((int row, IReadOnlyList<object> columns) in data[0])
            {
                if (row < 2)
                    continue;
                
                ulong id = Convert.ToUInt64(columns[0]);
                ulong unitId = Convert.ToUInt64(columns[1]);
                double probability = Convert.ToDouble(columns[2]);
                
                var constant = new GachaConstant(id, unitId, probability);
                
                m_data.Add(id, constant);
            }
        }
    }
}