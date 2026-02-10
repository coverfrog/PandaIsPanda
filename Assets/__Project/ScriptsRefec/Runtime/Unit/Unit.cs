using System.Collections.Generic;
using UnityEngine;

namespace PandaIsPanda
{
    public class Unit : MonoBehaviour
    {
        public UnitData UnitData { get; private set; }

        public void Setup
        (
            UnitData unitData
        )
        {
            UnitData = unitData;
        }
    }
}