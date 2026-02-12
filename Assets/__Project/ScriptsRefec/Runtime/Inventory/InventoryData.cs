using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class InventoryData
    {
        #region # Event

        public delegate void ItemUpdateHandler(List<ItemData> items);
        
        public event ItemUpdateHandler OnItemUpdate;

        #endregion
        
        [SerializeField] private List<ItemData> m_items = new();

        public IReadOnlyList<ItemData> Items => m_items;
        
        public void AddItem(ItemData addItem)
        {
            AddItem(new List<ItemData>() { addItem });
        }
        
        public void AddItem(List<ItemData> addItems)
        {
            foreach (ItemData addItem in addItems)
            {
                var find = m_items.FirstOrDefault(x => x.Constant.Id == addItem.Constant.Id);
                if (find != null)
                {
                    find.AddCount(addItem.Count);
                }

                else
                {
                    m_items.Add(addItem);
                }
            }
    
            OnItemUpdate?.Invoke(m_items);
        }

        public void RemoveItem(CountValue<ulong> removeItem)
        {
            RemoveItem(new List<CountValue<ulong>>() { removeItem });
        }

        public void RemoveItem(List<CountValue<ulong>> removeItems)
        {
            bool isChanged = false;

            foreach (CountValue<ulong> removeItem in removeItems)
            {
                var find = m_items.FirstOrDefault(x => x.Constant.Id == removeItem.value);
                if (find != null)
                {
                    // ReduceCount의 반환값이 남은 개수라고 가정
                    int remainingCount = find.ReduceCount(removeItem.count);
                    isChanged = true;

                    if (remainingCount <= 0)
                    {
                        // 개수가 0 이하면 리스트에서 완전히 삭제
                        m_items.Remove(find);
                    }
                }
            }

            if (isChanged)
            {
                OnItemUpdate?.Invoke(m_items);
            }
        }
    }
}