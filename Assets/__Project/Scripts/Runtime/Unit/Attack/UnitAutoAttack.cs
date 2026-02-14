using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace PandaIsPanda
{
    public class UnitAutoAttack : MonoBehaviour
    {
        private Unit m_owner;
        private Unit m_target;
        
        public UnitAutoAttack SetUnit(Unit unit)
        {
            m_owner = unit;
            return this;
        }

        public UnitAutoAttack AutoAttack()
        {
            StartCoroutine(CoAutoAttack());
            return this;
        }

        private bool Detect()
        {
            if (m_target)
            {
                if (m_target.UnitData.IsLive)
                {
                    return true;
                }

                m_target = null;
                
                return false;
            }

            var cols = Physics.OverlapSphere(transform.position, 10.0f);
            if (cols.Length == 0)
            {
                return false;
            }

            var enemyList = cols
                .Select(c => c.GetComponent<UnitColRelative>())
                .Where(ur => ur)
                .Where(ur => ur.Unit.UnitData.UnitCtrlType == UnitCtrlType.Enemy)
                .ToList();

            if (enemyList.Count == 0)
            {
                return false;
            }

            var enemy = enemyList
                .Select(ur => ur.Unit)
                .OrderBy(u => Vector3.Distance(u.transform.position, transform.position))
                .FirstOrDefault();

            m_target = enemy;
            m_target!.SetEvents(OnUnitIsLive, null);
            
            return true;
        }

        private void OnUnitIsLive(Unit sender, Unit owner, bool isLive)
        {
            if (!m_target)
                return;
            
            if (m_target != owner)
                return;

            m_target = null;
        }

        private void AttackTarget()
        {
            var result = m_owner.AttackTarget(m_target);
            
            if (result.isLive)
                return;

            m_target = null;
        }

        private IEnumerator CoAutoAttack()
        {
            var unitData = m_owner.UnitData;
            
            var isNormalAttack = unitData.Constant.IsNormalAttack;
            if (!isNormalAttack)
            {
                yield break;
            }

            var isCanNormalAttack = true;
            
            var stats = unitData.Stats;
            var normalAttackInter = 1.0f / stats[StatKey.k_normalAttackSpeed].Value;

            while (true)
            {
                if (isCanNormalAttack)
                {
                    if (Detect())
                    {
                        AttackTarget();
                        isCanNormalAttack = false;
                    }
                    else
                    {
                        for (float t = 0.0f; t < 0.01f; t += Time.deltaTime)
                        {
                            yield return null;
                        }
                    }
                }

                else
                {
                    for (float t = 0.0f; t < normalAttackInter; t += Time.deltaTime)
                    {
                        yield return null;
                    }

                    isCanNormalAttack = true;
                }
            }
        }

        private void OnDrawGizmos()
        {
            
        }
    }
}