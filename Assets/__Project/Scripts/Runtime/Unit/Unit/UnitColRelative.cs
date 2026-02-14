using UnityEngine;

namespace PandaIsPanda
{
    public class UnitColRelative : MonoBehaviour
    {
        [SerializeField] private Unit m_unit;
        
        public Unit Unit => m_unit;
    }
}