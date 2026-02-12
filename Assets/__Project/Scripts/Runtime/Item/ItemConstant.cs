using System;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class ItemConstant
    {
        [SerializeField] private ulong m_id;
        [SerializeField] private ulong m_nameId;
        [SerializeField] private string m_iconPath;
        
        public ulong Id => m_id;
        public ulong NameId => m_nameId;
        public string IconPath => m_iconPath;
        
        public ItemConstant(ulong id, ulong nameId, string iconPath)
        {
            m_id = id;
            m_nameId = nameId;
            m_iconPath = iconPath;
        }
    }
}