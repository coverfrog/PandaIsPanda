using System;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class ItemConstant
    {
        [SerializeField] private ulong m_id;
        [SerializeField] private ulong m_mergedId;
        [SerializeField] private uint m_grade;
        [SerializeField] private string m_subject;
        [SerializeField] private string m_description;
        [SerializeField] private Sprite m_icon;
        [SerializeField] private ItemType m_itemType;
        
        public ulong Id => m_id;
        public ulong MergedId => m_mergedId;
        
        public uint Grade => m_grade;
        
        public string Subject => m_subject;
        
        public string Description => m_description;
        
        public Sprite Icon => m_icon;
        
        public ItemType ItemType => m_itemType;
    }
}