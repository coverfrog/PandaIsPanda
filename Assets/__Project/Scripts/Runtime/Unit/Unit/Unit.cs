using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Pool;

namespace PandaIsPanda
{
    public class Unit : MonoBehaviour
    {
        #region # Event

        public delegate void OnIsLiveHandler(Unit sender, Unit owner, bool isLive);
        public delegate void OnHpHandler(Unit sender, Unit owner, float hp);
        
        public event OnIsLiveHandler OnIsLive;
        public event OnHpHandler OnHp;

        #endregion
        
        [Header("# References")]
        [SerializeField] private FollowerEntity m_followerEntity;

        [Header("# Runtime")]
        [SerializeField] private UnitData m_unitData;

        public UnitData UnitData => m_unitData;

        private IObjectPool<Unit> m_pool;

        public Unit SetData(UnitData unitData)
        {
            m_unitData = unitData;
            gameObject.name = $"{unitData.Constant.Id}_{unitData.Constant.DevName}";
            
            return this;
        }

        public Unit SetPool(IObjectPool<Unit> pool)
        {
            m_pool = pool; return this;
        }

        public Unit SetEvents(OnIsLiveHandler onIsLive, OnHpHandler onHp)
        {
            OnIsLive -= onIsLive;
            OnIsLive += onIsLive;
            
            OnHp -= onHp;
            OnHp += onHp;
            
            return this;
        }

        public Unit SetPosition(Vector3 position)
        {
            m_followerEntity.Teleport(position); 
            return this;
        }

        public UnitAttackResult AttackTarget(Unit target)
        {
            bool isLive = target.OnDamage(this);
            
            UnitAttackResult result = new UnitAttackResult()
            {
                isSuccess = true,
                isLive = isLive
            };
            
            return result;
        }

        public bool OnDamage(Unit sender)
        {
            var damage = sender.UnitData.Stats[StatKey.k_normalAttack].Value;
            
            //
            
            var orgHp = m_unitData.Stats[StatKey.k_hp].Value;
            var newHp = Mathf.Max(0, orgHp - damage);

            if (!Mathf.Approximately(orgHp, newHp))
            {
                OnHp?.Invoke(sender, this, newHp);
            }
            
            //

            var orgIsLive = m_unitData.IsLive;
            var newIsLive = m_unitData
                .SetHp(this, newHp)
                .SetIsLive(this, newHp > 0).IsLive;

            if (orgIsLive != newIsLive)
            {
                m_pool?.Release(this);
                OnIsLive?.Invoke(sender, this, newIsLive);
            }
            
            //
            
            var log = $"[{nameof(Unit)}] Hp: {orgHp} -> {newHp}";
            LogUtil.Log(log);
            
            return newIsLive;
        }
    }
}