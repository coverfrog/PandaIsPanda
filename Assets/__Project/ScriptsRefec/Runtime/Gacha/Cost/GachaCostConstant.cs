using System;
using System.Collections.Generic;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class GachaCostConstant
    {
        [SerializeField] private ulong m_id;
        [SerializeField] private List<CountValue<ulong>> m_costItems;

        public ulong Id => m_id;

        public IReadOnlyList<CountValue<ulong>> CostItems => m_costItems;
        
        public GachaCostConstant(ulong id, List<CountValue<ulong>> costItems)
        {
            m_id = id;
            m_costItems = costItems;
        }
    }
}