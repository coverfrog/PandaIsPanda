using System;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class UnitData
    {
        [SerializeField] private UnitConstant m_constant;
        [SerializeField] private UnityDictionary<ulong, StatData> m_stats = new();
        
        public UnitConstant Constant => m_constant;
        
        public UnityDictionary<ulong, StatData> Stats => m_stats;
        
        public UnitData(UnitConstant constant)
        {
            m_constant = constant;
            
            m_stats.Clear();
            
            m_stats.Add(StatKey.k_hp, 
                    new StatData(DataManager.Instance.StatConstants[StatKey.k_hp], 
                    constant.DefaultHpId));
            
            m_stats.Add(StatKey.k_mp, 
                    new StatData(DataManager.Instance.StatConstants[StatKey.k_mp], 
                        constant.DefaultMpId));
            
            m_stats.Add(StatKey.k_normalAttackSpeed, 
                    new StatData(DataManager.Instance.StatConstants[StatKey.k_normalAttackSpeed], 
                        constant.DefaultNormalSpeedAttackId));
        }
    }
}