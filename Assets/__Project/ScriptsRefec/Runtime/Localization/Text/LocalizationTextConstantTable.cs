using System;
using System.Collections.Generic;
using UnityEngine;

namespace PandaIsPanda
{
    public class LocalizationTextConstantTable : ScriptableObject, IConstantTable
    {
        [SerializeField] private UnityDictionary<ulong, LocalizationTextConstant> m_data = new();
        
        public IReadOnlyDictionary<ulong, LocalizationTextConstant> Data 
        {
            get
            {
                if (m_readOnlyData == null)
                    m_readOnlyData = m_data.ToReadOnlyDictionary();

                return m_readOnlyData;
            }
        }
        
        private IReadOnlyDictionary<ulong, LocalizationTextConstant> m_readOnlyData;
        
        public void Load(IReadOnlyDictionary<int, IReadOnlyDictionary<int, IReadOnlyList<object>>> data)
        {
            m_data.Clear();

            foreach ((int row, IReadOnlyList<object> columns) in data[0])
            {
                if (row < 2)
                    continue;
                
                ulong id = Convert.ToUInt64(columns[0]);
                string kr = Convert.ToString(columns[1]);
                string en = Convert.ToString(columns[2]);
                string fr = Convert.ToString(columns[3]);
                
                var constant = new LocalizationTextConstant(id, kr, en, fr);
                
                m_data.Add(id, constant);
            }
        }
    }
}