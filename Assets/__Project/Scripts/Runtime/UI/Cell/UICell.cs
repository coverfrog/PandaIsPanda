using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PandaIsPandaMvp
{
    public class UICell : MonoBehaviour
    {
        [Header("# References")] 
        [SerializeField] private RectTransform m_rt;
        [SerializeField] private Image m_imgBg;
        [SerializeField] private Image m_imgFocus;
        [Space] 
        [SerializeField] private UIItem m_uiItem;

        public RectTransform Rt => m_rt;

        
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

            if (item != null && gameObject.activeInHierarchy)
            {
                StartCoroutine(CoCoCo());
            }
            
            return this;
        }

        public UICell SetEnableIcon(Cell _, bool enable)
        {
            if (m_uiItem) m_uiItem.SetEnableIcon(enable);
            return this;
        }

        public UICell SetEnableFocus(Cell _, bool enable)
        {
            if (m_imgFocus) m_imgFocus.enabled = enable;
            return this;
        }

        public UICell SetPositionByScreen(Cell _, Vector2 screenPos, float duration)
        {
            if (m_rt && RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rtParent, screenPos, null,
                    out Vector2 localPos))
            {
                m_rt.DOAnchorPos(localPos, duration, true).SetEase(Ease.OutQuad);
            }
            return this;
        }
        
        #endregion
        
        public bool IsRectContains(Cell _, Vector2 screenPos)
        {
            return m_rt != null && RectTransformUtility.RectangleContainsScreenPoint(m_rt, screenPos);
        }

        private IEnumerator CoCoCo()
        {
            while (true)
            {
                for (float t = 0.0f; t < 1.0f; t += Time.deltaTime)
                {
                    yield return new WaitForEndOfFrame();
                }

                var mon =  FindAnyObjectByType<Monster>();
                if (mon == null)
                    continue;
                
                mon.Attack(50);
            }
        }
    }
}