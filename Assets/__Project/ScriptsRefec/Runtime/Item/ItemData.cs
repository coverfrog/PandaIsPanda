using System;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class ItemData
    {
        public ItemConstant Constant { get; }
        
        public int Count { get; private set; }

        public Sprite Icon
        {
            get
            {
                if (m_icon == null)
                    m_icon = AddressableUtil.Load<Sprite>(Constant.IconPath);
                
                return m_icon;
            }
        }

        private Sprite m_icon;
        
        public ItemData(ItemConstant constant, int count)
        {
            Constant = constant;
            Count = count;
        }
    }
}