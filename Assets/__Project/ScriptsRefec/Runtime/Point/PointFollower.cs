using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace PandaIsPanda
{
    public class PointFollower : MonoBehaviour
    {
        private IReadOnlyList<Vector3> m_points;
        
        public void Setup(IReadOnlyList<Vector3> points)
        {
            m_points = points;
        }

        public void Follow()
        {
            StartCoroutine(CoFollow());
        }

        private IEnumerator CoFollow()
        {
            if (m_points == null ||
                m_points.Count < 2)
            {
                yield break;
            }
            
            transform.position = m_points[0];
        }
    }
}