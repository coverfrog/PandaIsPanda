using System;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class UnitConstant
    {
        [SerializeField] private string m_devName;
        [SerializeField] private ulong m_id;
        [SerializeField] private ulong m_nameId;
        [SerializeField] private bool m_isNormalAttack;
        [SerializeField] private ulong m_defaultHpId;
        [SerializeField] private ulong m_defaultMpId;
        [SerializeField] private ulong m_defaultNormalAttackSpeedId;
        
        public string DevName => m_devName;
        public ulong Id => m_id;
        
        public ulong NameId => m_nameId;

        public bool IsNormalAttack => m_isNormalAttack;
        
        public ulong DefaultHpId => m_defaultHpId;
        
        public ulong DefaultMpId => m_defaultMpId;

        public ulong DefaultNormalSpeedAttackId => m_defaultNormalAttackSpeedId;

        public UnitConstant(string devName, ulong id, ulong nameId, bool isNormalAttack, ulong defaultHpId, ulong defaultMpId, ulong normalAttackSpeedId)
        {
            m_devName = devName;
            m_id = id;
            m_nameId = nameId;
            m_isNormalAttack = isNormalAttack;
            m_defaultHpId = defaultHpId;
            m_defaultMpId = defaultMpId;
            m_defaultNormalAttackSpeedId = normalAttackSpeedId;
        }
    }
}