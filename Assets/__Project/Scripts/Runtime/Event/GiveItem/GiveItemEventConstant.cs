using System;
using System.Collections.Generic;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class GiveItemEventConstant
    {
        [SerializeField] private ulong m_id;
        [SerializeField] private GiveItemEventTriggerType m_giveItemEventTriggerType;
        [SerializeField] private List<CountValue<ulong>> m_giveItems;
        
        public ulong Id => m_id;
        
        public GiveItemEventTriggerType GiveItemEventTriggerType => m_giveItemEventTriggerType;
        
        public IReadOnlyList<CountValue<ulong>> GiveItems => m_giveItems;
        
        public GiveItemEventConstant(ulong id, GiveItemEventTriggerType giveItemEventTriggerType, List<CountValue<ulong>> giveItems)
        {
            m_id = id;
            m_giveItemEventTriggerType = giveItemEventTriggerType;
            m_giveItems = giveItems;
        }
    }
}