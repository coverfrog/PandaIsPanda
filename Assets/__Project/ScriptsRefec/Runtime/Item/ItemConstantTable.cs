using System;
using System.Collections.Generic;
using UnityEngine;

namespace PandaIsPanda
{
    public class ItemConstantTable : ScriptableObject, IConstantTable
    {
        [SerializeField] private UnityDictionary<ulong, ItemConstant> m_data = new();

        public IReadOnlyDictionary<ulong, ItemConstant> Data 
        {
            get
            {
                if (m_readOnlyData == null)
                    m_readOnlyData = m_data.ToReadOnlyDictionary();

                return m_readOnlyData;
            }
        }
        
        private IReadOnlyDictionary<ulong, ItemConstant> m_readOnlyData;
        
        public void Load(IReadOnlyDictionary<int, IReadOnlyDictionary<int, IReadOnlyList<object>>> data)
        {
            m_data.Clear();

            foreach ((int row, IReadOnlyList<object> columns) in data[0])
            {
                if (row < 2)
                    continue;
                
                ulong id = Convert.ToUInt64(columns[0]);
                ulong nameId = Convert.ToUInt64(columns[1]);
                string iconPath = Convert.ToString(columns[2]);
                
                var constant = new ItemConstant(id, nameId, iconPath);
                
                m_data.Add(id, constant);
            }
        }
    }
}