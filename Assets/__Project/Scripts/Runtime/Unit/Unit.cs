using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace PandaIsPanda
{
    public class Unit : MonoBehaviour
    {
        [Header("# References")]
        [SerializeField] private FollowerEntity m_followerEntity;
        
        public UnitData UnitData { get; private set; }

        public Unit Setup(UnitData unitData)
        {
            UnitData = unitData; 
            return this;
        }

        public Unit SetPosition(Vector3 position)
        {
            m_followerEntity.Teleport(position); 
            return this;
        }
    }
}