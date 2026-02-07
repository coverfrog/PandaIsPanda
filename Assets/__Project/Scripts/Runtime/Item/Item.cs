using System;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class Item : IDeepClone<Item>
    {
        [SerializeField] private ItemConstant m_constant;
        
        public ItemConstant Constant => m_constant;
        
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