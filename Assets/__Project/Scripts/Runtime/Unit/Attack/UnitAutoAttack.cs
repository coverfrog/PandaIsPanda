using System;
using System.Collections;
using UnityEngine;

namespace PandaIsPanda
{
    public class UnitAutoAttack : MonoBehaviour
    {
        private Unit m_unit;
        
        public UnitAutoAttack SetUnit(Unit unit)
        {
            m_unit = unit;
            return this;
        }

        public UnitAutoAttack AutoAttack()
        {
            StartCoroutine(CoAutoAttack());
            return this;
        }

        private void Detect()
        {
            
        }

        private IEnumerator CoAutoAttack()
        {
            var unitData = m_unit.UnitData;
            
            UnityDictionary<ulong, StatData> stats = unitData.Stats;

            var isNormalAttack = unitData.Constant.IsNormalAttack;
            var normalAttackInter = 1.0f / stats[StatKey.k_normalAttackSpeed].Value.Value;

            for (float t = 0.0f; t < normalAttackInter; t += Time.deltaTime)
            {
                yield return null;
            }
        }

        private void OnDrawGizmos()
        {
            
        }
    }
}