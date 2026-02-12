using System;

namespace PandaIsPanda
{
    [Serializable]
    public class SpawnEventData
    {
        public SpawnEventConstant Constant { get; }
        
        public int CallCount { get; private set; }
        
        public SpawnEventData(SpawnEventConstant constant)
        {
            Constant = constant;
        }

        public bool CheckCondition(RoundData roundData)
        {
            if (Constant.SpawnEventTriggerType == SpawnEventTriggerType.RoundBegin)
                return CallCount < Constant.CallCount;
            
            return false;
        }

        public SpawnEventData AddCallCount()
        {
            CallCount++; return this;
        }
    }
}