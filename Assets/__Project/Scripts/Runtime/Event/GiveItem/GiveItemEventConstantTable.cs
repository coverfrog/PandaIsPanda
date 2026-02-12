using System;
using System.Collections.Generic;
using UnityEngine;

namespace PandaIsPanda
{
    public class GiveItemEventConstantTable : ScriptableObject, IConstantTable
    {
        [SerializeField] private UnityDictionary<ulong, GiveItemEventConstant> m_data = new();
        
        public IReadOnlyDictionary<ulong, GiveItemEventConstant> Data 
        {
            get
            {
                if (m_readOnlyData == null)
                    m_readOnlyData = m_data.ToReadOnlyDictionary();

                return m_readOnlyData;
            }
        }
        
        private IReadOnlyDictionary<ulong, GiveItemEventConstant> m_readOnlyData;
        
        public void Load(IReadOnlyDictionary<int, IReadOnlyDictionary<int, IReadOnlyList<object>>> data)
        {
            m_data.Clear();

            foreach ((int row, IReadOnlyList<object> columns) in data[0])
            {
                if (row < 2)
                    continue;
                
                ulong id = Convert.ToUInt64(columns[0]);
                GiveItemEventTriggerType triggerType = (GiveItemEventTriggerType)Convert.ToInt32(columns[1]);
                
                int length = columns.Count;
                List<CountValue<ulong>> giveItems = new List<CountValue<ulong>>();
                for (int i = 2; i < length; i++)
                {
                    var split = Convert.ToString(columns[i]).Split('_');
                    if (split.Length < 2)
                        continue;
                    
                    var itemId = Convert.ToUInt64(split[0]);
                    var itemCount = Convert.ToInt32(split[1]);

                    CountValue<ulong> giveItem = new CountValue<ulong>()
                    {
                        value = itemId,
                        count = itemCount
                    };
                    
                    giveItems.Add(giveItem);
                }
                
                var constant = new GiveItemEventConstant(id, triggerType, giveItems);
                
                m_data.Add(id, constant);
            }
        }
    }
}