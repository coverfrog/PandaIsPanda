using System;
using System.Collections.Generic;
using UnityEngine;

namespace PandaIsPanda
{
    public class UnitConstantTable : ScriptableObject, IConstantTable
    {
        [SerializeField] private UnityDictionary<ulong, UnitConstant> m_data = new();

        public IReadOnlyDictionary<ulong, UnitConstant> Data
        {
            get
            {
                if (m_readOnlyData == null)
                    m_readOnlyData = m_data.ToReadOnlyDictionary();

                return m_readOnlyData;
            }
        }
        
        private IReadOnlyDictionary<ulong, UnitConstant> m_readOnlyData;
        
        public void Load(IReadOnlyDictionary<int, IReadOnlyDictionary<int, IReadOnlyList<object>>> data)
        {
            m_data.Clear();

            foreach ((int row, IReadOnlyList<object> columns) in data[0])
            {
                if (row < 2)
                    continue;
                
                ulong id = Convert.ToUInt64(columns[0]);
                string devName = Convert.ToString(columns[1]);
                ulong nameId = Convert.ToUInt64(columns[2]);
                ulong defaultHpId = Convert.ToUInt64(columns[3]);
                ulong defaultMpId = Convert.ToUInt64(columns[4]);
                
                var constant = new UnitConstant(devName, id, nameId, defaultHpId, defaultMpId);
                
                m_data.Add(id, constant);
            }
        }
    }
}