using System;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class UnitConstant
    {
        [SerializeField] private ulong m_id;
        [SerializeField] private ulong m_nameId;
        
        public ulong Id => m_id;
        
        public ulong NameId => m_nameId;

        public UnitConstant(ulong id, ulong nameId)
        {
            m_id = id;
            m_nameId = nameId;
        }
    }
}