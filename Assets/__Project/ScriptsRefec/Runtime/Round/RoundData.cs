using System;

namespace PandaIsPanda
{
    [Serializable]
    public class RoundData
    {
        public RoundConstant Constant { get; }
        
        public int TimerSecInt { get; private set; }
        
        public RoundData(RoundConstant constant)
        {
            Constant = constant;
            TimerSecInt = Convert.ToInt32(constant.Duration);
        }

        public RoundData SetTimerSec(int timerSec)
        {
            TimerSecInt = timerSec; return this;
        }
    }
}