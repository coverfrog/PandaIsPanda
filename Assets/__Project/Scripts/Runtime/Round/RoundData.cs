using System;
using System.Collections.Generic;

namespace PandaIsPanda
{
    [Serializable]
    public class RoundData
    {
        public RoundConstant Constant { get; }

        public IReadOnlyDictionary<ulong, SpawnEventData> SpawnEventData { get; }
        
        public IReadOnlyDictionary<ulong, GiveItemEventData> GiveItemEventData { get; }

        public int TimerSecInt { get; private set; }
        
        public float TimerSec { get; private set; }

        public RoundData
        (
            RoundConstant constant,
            IReadOnlyDictionary<ulong, SpawnEventConstant> spawnEventConstants,
            IReadOnlyDictionary<ulong, GiveItemEventConstant> giveItemEventConstants)
        {
            Constant = constant;
            TimerSecInt = Convert.ToInt32(constant.Duration);
            TimerSec = constant.Duration;

            Dictionary<ulong, SpawnEventData> spawnEventData = new Dictionary<ulong, SpawnEventData>();
            foreach (ulong spawnEventId in Constant.SpawnEventIds)
            {
                if (spawnEventId != 0 &&
                    spawnEventConstants != null &&
                    spawnEventConstants.TryGetValue(spawnEventId, out var spawnEventConstant))
                {
                    spawnEventData.Add(spawnEventId, new SpawnEventData(spawnEventConstant));
                }
            }
            
            SpawnEventData = spawnEventData;
            
            Dictionary<ulong, GiveItemEventData> giveItemEventData = new Dictionary<ulong, GiveItemEventData>();
            foreach (ulong giveItemEventId in Constant.GiveItemEventIds)
            {
                if (giveItemEventId != 0 &&
                    giveItemEventConstants != null &&
                    giveItemEventConstants.TryGetValue(giveItemEventId, out var giveItemEventConstant))
                {
                    giveItemEventData.Add(giveItemEventId, new GiveItemEventData(giveItemEventConstant));
                }
            }
            
            GiveItemEventData = giveItemEventData;
        }

        public RoundData SetTimerSecInt(int timerSecInt)
        {
            TimerSecInt = timerSecInt; return this;
        }

        public RoundData SetTimerSec(float timerSec)
        {
            TimerSec = timerSec; return this;
        }
    }
}