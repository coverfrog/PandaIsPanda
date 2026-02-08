using System;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class InventorySlotData
    {
        [SerializeField] private ItemData m_itemData;

        public ItemData ItemData => m_itemData;

        public InventorySlotData(ItemData itemData)
        {
            m_itemData = itemData;
        }
    }
}