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
        [SerializeField] private string m_iconPath;
        [SerializeField] private bool m_isPercentType;
        [SerializeField] private UnityDictionary<ulong, float> m_defaultStats = new();
        
        public ulong Id => m_id;
        
        public ulong NameId => m_nameId;

        public Sprite Icon
        {
            get
            {
                if (m_icon != null) return m_icon;
                return m_icon = AddressableUtil.Load<Sprite>(m_iconPath);
            }
        }

        private Sprite m_icon;
        
        public bool IsPercentType => m_isPercentType;

        public IReadOnlyDictionary<ulong, float> DefaultStats
        {
            get
            {
                if (m_defaultStats != null) return m_defaultStatsReadOnly;
                return m_defaultStatsReadOnly = m_defaultStats.ToReadOnlyDictionary();
            }
        }
        
        private IReadOnlyDictionary<ulong, float> m_defaultStatsReadOnly;
        
        public StatConstant(string devName, ulong id, ulong nameId, string iconPath, bool isPercentType, UnityDictionary<ulong, float> defaultStats)
        {
            m_devName = devName;
            m_id = id;
            m_nameId = nameId;
            m_iconPath = iconPath;
            m_isPercentType = isPercentType;
            m_defaultStats = defaultStats;
        }
    }
}