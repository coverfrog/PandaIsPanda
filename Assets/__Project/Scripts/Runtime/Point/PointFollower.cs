using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pathfinding;
using UnityEngine;

namespace PandaIsPanda
{
    public class PointFollower : MonoBehaviour
    {
        private FollowerEntity m_followerEntity;
        
        private IReadOnlyList<Vector3> m_points;

        public PointFollower SetPoints(IReadOnlyList<Vector3> points)
        {
            m_followerEntity = GetComponent<FollowerEntity>();
            m_points = points;
   
            return this;
        }

        public void Follow()
        {
            if (m_points == null ||
                m_points.Count < 2 ||
                !m_followerEntity ||
                !gameObject.activeInHierarchy)
            {
                return;
            }
            
            StartCoroutine(CoFollow());
        }

        private IEnumerator CoFollow()
        {
            m_followerEntity.Teleport(m_points[0]);

            // 1. Vector3 리스트를 기반으로 새로운 경로 객체 생성
            // ABPath는 A지점에서 B지점으로 가는 경로를 의미합니다.
            var path = ABPath.Construct(m_points[0], m_points[^1]);
    
            // 2. 경로의 노드 리스트를 강제로 주입 (Vector3 -> Path 노드로 변환 과정이 필요할 수 있음)
            // 하지만 가장 간단한 방법은 아래처럼 '이동할 포인트'를 직접 큐에 넣는 방식입니다.
    
            // FollowerEntity는 내부적으로 자동 경로 재계산을 하므로 
            // 수동 제어를 위해 자동 업데이트를 잠시 끄는 것이 좋습니다.
            var repath = m_followerEntity.autoRepath;
            repath.mode = AutoRepathPolicy.Mode.Never;
            m_followerEntity.autoRepath = repath;

            int cursor = 0;
            while (true)
            {
                m_followerEntity.destination = m_points[cursor];
        
                // 경로가 계산될 때까지 기다릴 필요 없이 즉시 다음 포인트로 부드럽게 이어지길 원하신다면
                // reachedEndOfPath 대신 거리를 체크하는 방식이 더 유연합니다.
                while (m_followerEntity.remainingDistance > 0.1f)
                {
                    yield return null;
                }

                cursor = (cursor + 1) % m_points.Count;
        
                // 다음 목적지 설정 후 즉시 경로 업데이트
                m_followerEntity.SearchPath(); 
            }
        }
    }
}