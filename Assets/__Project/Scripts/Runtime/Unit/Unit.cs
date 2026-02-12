using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace PandaIsPanda
{
    public class Unit : MonoBehaviour
    {
        [Header("# References")]
        [SerializeField] private FollowerEntity m_followerEntity;

        [Header("# Runtime")]
        [SerializeField] private UnitData m_unitData;

        public UnitData UnitData => m_unitData;
        
        public Unit Setup(UnitData unitData)
        {
            m_unitData = unitData;
            
            gameObject.name = $"{unitData.Constant.Id}_{unitData.Constant.DevName}";
            
            return this;
        }

        public Unit SetPosition(Vector3 position)
        {
            m_followerEntity.Teleport(position); 
            return this;
        }
    }
}