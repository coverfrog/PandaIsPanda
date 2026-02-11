using System;
using System.Collections.Generic;
using UnityEngine;

namespace PandaIsPanda
{
    public class GachaCostConstantTable : ScriptableObject, IConstantTable
    {
        [SerializeField] private UnityDictionary<ulong, GachaCostConstant> m_data = new();
        
        public IReadOnlyDictionary<ulong, GachaCostConstant> Data 
        {
            get
            {
                if (m_readOnlyData == null)
                    m_readOnlyData = m_data.ToReadOnlyDictionary();

                return m_readOnlyData;
            }
        }
        
        private IReadOnlyDictionary<ulong, GachaCostConstant> m_readOnlyData;

        
        public void Load(IReadOnlyDictionary<int, IReadOnlyDictionary<int, IReadOnlyList<object>>> data)
        {
            m_data.Clear();

            foreach ((int row, IReadOnlyList<object> columns) in data[0])
            {
                if (row < 2)
                    continue;
                
                ulong id = Convert.ToUInt64(columns[0]);
                
                int length = columns.Count;
                List<CountValue<ulong>> costItems = new List<CountValue<ulong>>();
                for (int i = 1; i < length; i++)
                {
                    var split = Convert.ToString(columns[i]).Split('_');
                    if (split.Length < 2)
                        continue;
                    
                    var itemId = Convert.ToUInt64(split[0]);
                    var itemCount = Convert.ToInt32(split[1]);

                    CountValue<ulong> costItem = new CountValue<ulong>()
                    {
                        value = itemId,
                        count = itemCount
                    };
                    
                    costItems.Add(costItem);
                }
                
                var constant = new GachaCostConstant(id, costItems);
                
                m_data.Add(id, constant);
            }
        }
    }
}