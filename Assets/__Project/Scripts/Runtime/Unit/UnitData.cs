using System;

namespace PandaIsPanda
{
    [Serializable]
    public class UnitData
    {
        public UnitConstant Constant { get; }
        
        public UnityDictionary<ulong, StatData> Stats { get; } = new UnityDictionary<ulong, StatData>();
        
        public UnitData(UnitConstant constant)
        {
            Constant = constant;
            
            Stats.Add(StatKey.k_hp, new StatData(DataManager.Instance.StatConstants[StatKey.k_hp], constant.DefaultHpId));
            Stats.Add(StatKey.k_mp, new StatData(DataManager.Instance.StatConstants[StatKey.k_mp], constant.DefaultMpId));
        }
    }
}