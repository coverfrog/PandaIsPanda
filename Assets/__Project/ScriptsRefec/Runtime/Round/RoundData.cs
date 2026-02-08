using System;

namespace PandaIsPanda
{
    [Serializable]
    public class RoundData
    {
        public RoundConstant Constant { get; }
        
        public int TimerSec { get; private set; }
        
        public RoundData(RoundConstant constant)
        {
            Constant = constant;
            TimerSec = Convert.ToInt32(constant.Duration);
        }

        public RoundData SetTimerSec(int timerSec)
        {
            TimerSec = timerSec; return this;
        }
    }
}