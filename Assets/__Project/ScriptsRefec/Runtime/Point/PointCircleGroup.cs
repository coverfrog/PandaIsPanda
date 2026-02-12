using System;
using System.Collections.Generic;
using UnityEngine;

namespace PandaIsPanda
{
    public class PointCircleGroup : MonoBehaviour
    {
        [Header("# Options")] 
        [SerializeField, Min(3)] private int m_edgeCount = 30;
        [SerializeField] private float m_radius = 10.0f;
        [SerializeField] private Color m_gizmoColor = Color.green;
        
        [Header("# Data")]
        [SerializeField] private List<Vector3> m_points = new();
        
        public IReadOnlyList<Vector3> Points => m_points;

        public PointCircleGroup SetEdgeCount(int edgeCount)
        {
            m_edgeCount = edgeCount;
            return this;
        }
        
        public PointCircleGroup Spread()
        {
            const float tau = Mathf.PI * 2.0f;

            var delta = tau / m_edgeCount;

            m_points.Clear();
            
            for (int i = 0; i < m_edgeCount; i++)
            {
                float x = Mathf.Sin(delta * i);
                float z = Mathf.Cos(delta * i);
                
                Vector3 point = transform.position + 
                                new Vector3(x, 0, z) * m_radius;
                
                m_points.Add(point);
            }       
            
            return this;
        }

        private void OnDrawGizmos()
        {
            if (m_points == null ||
                m_points.Count < 2)
                return;

            for (var i = 0; i < m_points.Count; i++)
            {
                Vector3 p0 = m_points[i];
                Vector3 p1 = m_points[(i + 1) % m_points.Count];
                
                Gizmos.color = m_gizmoColor;
                Gizmos.DrawLine(p0, p1);
            }
        }
    }
}