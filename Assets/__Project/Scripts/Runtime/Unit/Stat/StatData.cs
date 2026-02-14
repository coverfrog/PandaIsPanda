using System;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class StatData
    {
        [SerializeField, ReadOnly] private string m_devName;
        [SerializeField, ReadOnly] private float m_value;
        [SerializeField, ReadOnly] private StatConstant m_constant;

        public StatConstant Constant => m_constant;
        
        public float Value => m_value;
        
        public StatData(StatConstant statConstant, ulong defaultId)
        {
            m_constant = statConstant;
            m_devName = statConstant.DevName;
            m_value = statConstant.DefaultStats[defaultId];
        }

        public StatData SetValue(float newValue)
        {
            m_value = newValue; return this;
        }
    }
}