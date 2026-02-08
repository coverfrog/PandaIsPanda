using System;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class SpawnEventConstant
    {
        [SerializeField] private ulong m_id;
        
        public ulong Id => m_id;

        public SpawnEventConstant(ulong id)
        {
            m_id = id;
        }
    }
}