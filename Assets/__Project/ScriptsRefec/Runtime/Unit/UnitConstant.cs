using System;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class UnitConstant
    {
        [SerializeField] private ulong m_id;
        
        public ulong Id => m_id;

        public UnitConstant(ulong id)
        {
            m_id = id;
        }
    }
}