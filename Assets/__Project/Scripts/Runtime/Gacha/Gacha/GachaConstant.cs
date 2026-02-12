using System;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class GachaConstant
    {
        [SerializeField] private ulong m_id;
        [SerializeField] private ulong m_unitId;
        [SerializeField] private double m_probability;
        
        public ulong Id => m_id;
        
        public ulong UnitId => m_unitId;
        
        public double Probability => m_probability;

        public GachaConstant
        (
            ulong id,
            ulong unitId,
            double probability
        )
        {
            m_id = id;
            m_unitId = unitId;
            m_probability = probability;
        }
    }
}