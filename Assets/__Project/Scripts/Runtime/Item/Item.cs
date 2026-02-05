using System;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class Item : IDeepClone<Item>
    {
        [SerializeField] private ItemConstant m_constant;
        [SerializeField] private ulong m_id;
        
        public ItemConstant Constant => m_constant;
        
        public ulong Id => m_id;
        
        public Item(ItemConstant constant)
        {
            m_constant = constant;
        }

        public virtual bool TryUse(out byte errorCode)
        {
            errorCode = 0;
            
            return errorCode == 0;
        }

        public Item DeepClone()
        {
            return new Item(m_constant);
        }
    }
}