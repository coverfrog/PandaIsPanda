using System;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class LocalizationTextConstant
    {
        [SerializeField] private ulong m_id;
        [SerializeField] private string m_kr;
        [SerializeField] private string m_en;
        [SerializeField] private string m_fr;
        
        public ulong Id => m_id;
        public string Kr => m_kr;
        public string En => m_en;
        public string Fr => m_fr;

        public LocalizationTextConstant(ulong id, string kr, string en, string fr)
        {
            m_id = id;
            m_kr = kr;
            m_en = en;
            m_fr = fr;
        }
    }
}