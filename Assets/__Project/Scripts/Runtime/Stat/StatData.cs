using System;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class StatData
    {
        [SerializeField] private string m_devName;
        [SerializeField] private ReactiveProperty<float> m_value;
        
        public StatConstant Constant { get; }
        
        public StatData(StatConstant statConstant, ulong defaultId)
        {
            Constant = statConstant;

            m_devName = statConstant.DevName;
            m_value = new ReactiveProperty<float>(statConstant.DefaultStats[defaultId]);
        }

        public void SetValue(float value)
        {
            m_value.Value = value;
        }
    }
}