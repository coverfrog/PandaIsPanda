using System.Collections.Generic;
using UnityEngine;

namespace PandaIsPanda
{
    [CreateAssetMenu(fileName = nameof(ItemConstantTable), menuName = "PandaIsPanda/ItemConstTable")]
    public class ItemConstantTable : ScriptableObject
    {
        [SerializeField] private List<ItemConstant> m_constList = new();
        
        public IReadOnlyList<ItemConstant> ConstList => m_constList;
    }
}