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

        private bool Detect(out Unit unit)
        {
            if (m_target)
            {
                if (m_target.UnitData.IsLive)
                {
                    unit = m_target;
                    return true;
                }
                
                else
                {
                    m_target = unit = null;
                    return false;
                }
            }

            else
            {
                var cols = Physics.OverlapSphere(transform.position, 10.0f);
                if (cols.Length == 0)
                {
                    unit = null;
                    return false;
                }

                var enemyList = cols
                    .Select(c => c.GetComponent<UnitColRelative>())
                    .Where(ur => ur)
                    .Where(ur => ur.Unit.UnitData.UnitCtrlType == UnitCtrlType.Enemy)
                    .ToList();

                if (enemyList.Count == 0)
                {
                    unit = null;
                    return false;
                }

                var enemy = enemyList
                    .Select(ur => ur.Unit)
                    .OrderBy(u => Vector3.Distance(u.transform.position, transform.position))
                    .FirstOrDefault();

                m_target = unit = enemy;
                return true;
            }
        }

        private void Attack(Unit unit)
        {
            LogUtil.Log("공격?");
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
            var normalAttackInter = 1.0f / stats[StatKey.k_normalAttackSpeed].Value.Value;

            while (true)
            {
                if (isCanNormalAttack)
                {
                    if (Detect(out Unit unit))
                    {
                        Attack(unit);
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