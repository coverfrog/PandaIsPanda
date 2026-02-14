using System;
using System.Collections.Generic;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class StatConstant
    {
        [SerializeField] private string m_devName;
        [SerializeField] private ulong m_id;
        [SerializeField] private ulong m_nameId;
        [SerializeField] private bool m_isPercentType;
        [SerializeField] private UnityDictionary<ulong, float> m_defaultStats;
        
        public string DevName => m_devName;
        public ulong Id => m_id;
        
        public ulong NameId => m_nameId;

        public bool IsPercentType => m_isPercentType;

        public IReadOnlyDictionary<ulong, float> DefaultStats => m_defaultStats.ToReadOnlyDictionary();
        
        public StatConstant(string devName, ulong id, ulong nameId, bool isPercentType, UnityDictionary<ulong, float> defaultStats)
        {
            m_devName = devName;
            m_id = id;
            m_nameId = nameId;
            m_isPercentType = isPercentType;
            m_defaultStats = defaultStats;
        }
    }
}