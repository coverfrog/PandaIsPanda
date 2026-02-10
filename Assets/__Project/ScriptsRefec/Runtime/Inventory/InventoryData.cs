using System;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class InventoryData
    {
        [SerializeField] private ulong m_id;

        public ulong Id => m_id;
        
        public InventoryData(ulong id)
        {
           m_id = id;
        }
    }
}