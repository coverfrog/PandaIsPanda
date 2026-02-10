using System;
using System.Collections.Generic;
using UnityEngine;

namespace PandaIsPanda
{
    public class UIGoodsItemViewGroup : MonoBehaviour
    {
        [SerializeField] private List<UIGoodsItemView> m_data = new();

        private void OnEnable()
        {
            foreach (UIGoodsItemView uiGoodsItemView in m_data)
            {
                
            }
        }
    }
}