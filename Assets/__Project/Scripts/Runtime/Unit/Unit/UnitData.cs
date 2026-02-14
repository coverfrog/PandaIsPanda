using System;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class UnitData
    {
        [SerializeField] private UnitConstant m_constant;
        [SerializeField] private bool m_isLive = true;
        [SerializeField] private UnitCtrlType m_unitCtrlType;
        [SerializeField] private UnityDictionary<ulong, StatData> m_stats = new();
        
        public UnitConstant Constant => m_constant;
        
        public bool IsLive => m_isLive;

        public UnitCtrlType UnitCtrlType => m_unitCtrlType;
        
        public UnityDictionary<ulong, StatData> Stats => m_stats;

        public UnitData
        (
            UnitConstant constant,
            UnitCtrlType unitCtrlType
        )
        {
            m_constant = constant;
            m_unitCtrlType = unitCtrlType;
            
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
            
            m_stats.Add(StatKey.k_normalAttack, 
                new StatData(DataManager.Instance.StatConstants[StatKey.k_normalAttack], 
                    constant.DefaultNormalAttackId));
        }
        
        public UnitData SetIsLive(Unit _, bool isLive)
        {
            m_isLive = isLive; return this;
        }

        public UnitData SetHp(Unit _, float hp)
        {
            m_stats[StatKey.k_hp].SetValue(hp); return this;
        }
    }
}