using System;
using System.Collections.Generic;
using UnityEngine;

namespace PandaIsPanda
{
    public class StatConstantTable : ScriptableObject, IConstantTable
    {
        [SerializeField] private UnityDictionary<ulong, StatConstant> m_data = new();

        public IReadOnlyDictionary<ulong, StatConstant> Data 
        {
            get
            {
                if (m_readOnlyData == null)
                    m_readOnlyData = m_data.ToReadOnlyDictionary();

                return m_readOnlyData;
            }
        }
        
        private IReadOnlyDictionary<ulong, StatConstant> m_readOnlyData;
        
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
                bool isPercentType = Convert.ToBoolean(columns[3]);
                int sheetIndex = Convert.ToInt32(columns[4]);

                UnityDictionary<ulong, float> defaultValues = new();
                foreach ((int row2, IReadOnlyList<object> columns2) in data[sheetIndex])
                {
                    if (row2 < 2)
                        continue;
                    
                    ulong level = Convert.ToUInt64(columns2[0]);
                    float value = Convert.ToSingle(columns2[1]);

                    defaultValues.Add(level, value);
                }

                var constant = new StatConstant(devName, id, nameId, isPercentType, defaultValues);

                m_data.Add(id, constant);
            }
        }
    }
}