using System;

namespace PandaIsPanda
{
    [Serializable]
    public class StatData
    {
        public StatConstant Constant { get; }

        public ReactiveProperty<float> Value { get; } 
        
        public StatData(StatConstant statConstant, ulong defaultId)
        {
            Constant = statConstant;
            Value = new ReactiveProperty<float>(statConstant.DefaultStats[defaultId]);
        }

        public void SetValue(float value)
        {
            Value.Value = value;
        }
    }
}