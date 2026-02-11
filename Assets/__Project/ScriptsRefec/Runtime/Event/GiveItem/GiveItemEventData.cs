using System;

namespace PandaIsPanda
{
    [Serializable]
    public class GiveItemEventData
    {
        public GiveItemEventConstant Constant { get; }

        public bool IsGive { get; private set; }
        
        public GiveItemEventData
        (
            GiveItemEventConstant constant
        )
        {
            Constant = constant;
        }
        
        public bool CheckCondition(RoundData roundData)
        {
            if (Constant.GiveItemEventTriggerType == GiveItemEventTriggerType.RoundBegin)
                return !IsGive;
            
            return false;
        }

        public void Call()
        {
            IsGive = true;
        }
    }
}