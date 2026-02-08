using System.Collections.Generic;
using UnityEngine;

namespace PandaIsPanda
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<InventorySlotData> m_slotData = new();

        public void AddItem(ulong constantItemId, int count)
        {
            var itemConstant = DataManager.Instance.ItemConstants[constantItemId];
            var itemData = new ItemData(itemConstant, count);
            var inventorySlotData = new InventorySlotData(itemData);
            
            m_slotData.Add(inventorySlotData);
        }
    }
}