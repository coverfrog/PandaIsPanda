using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PandaIsPanda
{
    public class UICell : MonoBehaviour
    {
        [Header("# References")] 
        [SerializeField] private RectTransform m_rt;
        [SerializeField] private Image m_imgBg;
        [Space] 
        [SerializeField] private UIItem m_uiItem;
        
        private RectTransform m_rtParent;
        
        #region # Set

        public UICell SetCursor(UIBoard _, float cellSize, RectTransform rtParent)
        {
            if (m_rt)
            {
                m_rt.anchorMin = Vector2.one * 0.5f;
                m_rt.anchorMax = Vector2.one * 0.5f;
                m_rt.pivot = Vector2.one * 0.5f;
                
                m_rt.sizeDelta = Vector2.one * cellSize;
            }
            
            m_rtParent = rtParent;
            
            return this;
        }
        
        public UICell SetName(UIBoard _, string str)
        {
            gameObject.name = str;
            return this;
        }
        
        public UICell SetBgColor(UIBoard _, Color color)
        {
            if (m_imgBg) m_imgBg.color = color;
            return this;
        }
        
        public UICell SetItem(Cell _, Item item)
        {
            if (m_uiItem) m_uiItem.SetItem(item);
            return this;
        }

        public UICell SetEnableIcon(Cell _, bool enable)
        {
            if (m_uiItem) m_uiItem.SetEnableIcon(enable);
            return this;
        }

        public UICell SetAnchorPosition(Cell _, Vector2 screenPos)
        {
            if (m_rt && RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rtParent, screenPos, null, out Vector2 localPos)) 
                m_rt.anchoredPosition = localPos;
            return this;
        }
        
        #endregion
        
        public bool IsRectContains(Cell _, Vector2 screenPos)
        {
            return m_rt != null && RectTransformUtility.RectangleContainsScreenPoint(m_rt, screenPos);
        }
    }
}