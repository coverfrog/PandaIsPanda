using System;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class UnitConstant
    {
        [SerializeField] private string m_devName;
        [SerializeField] private ulong m_id;
        [SerializeField] private ulong m_nameId;
        [SerializeField] private ulong m_defaultHpId;
        [SerializeField] private ulong m_defaultMpId;
        
        public ulong Id => m_id;
        
        public ulong NameId => m_nameId;
        
        public ulong DefaultHpId => m_defaultHpId;
        
        public ulong DefaultMpId => m_defaultMpId;

        public UnitConstant(string devName, ulong id, ulong nameId, ulong defaultHpId, ulong defaultMpId)
        {
            m_devName = devName;
            m_id = id;
            m_nameId = nameId;
            m_defaultHpId = defaultHpId;
            m_defaultMpId = defaultMpId;
        }
    }
}