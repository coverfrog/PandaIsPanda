using System.Collections.Generic;
using UnityEngine;

namespace PandaIsPandaMvp
{
    [CreateAssetMenu(fileName = nameof(RoundConstantTable), menuName = "PandaIsPanda/RoundConstantTable")]
    public class RoundConstantTable : ScriptableObject
    {
        [SerializeField] private List<RoundConstant> m_constantList = new();
        
        public IReadOnlyList<RoundConstant> ConstantList => m_constantList;
    }
}