using System;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class StatData
    {
        [SerializeField, ReadOnly] private string m_devName;
        [SerializeField, ReadOnly] private ReactiveProperty<float> m_value;
        [SerializeField, ReadOnly] private StatConstant m_constant;

        public StatConstant Constant => m_constant;
        
        public ReactiveProperty<float> Value => m_value;
        
        public StatData(StatConstant statConstant, ulong defaultId)
        {
            m_constant = statConstant;
            m_devName = statConstant.DevName;
            m_value = new ReactiveProperty<float>(statConstant.DefaultStats[defaultId]);
        }
    }
}