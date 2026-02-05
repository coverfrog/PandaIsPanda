using System;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class Cell
    {
        private int m_row;
        private int m_column;
        
        private bool m_isCursor;
        
        private Item m_item;
        private UICell m_uiCell;

        public Item Item => m_item;

        public RectTransform Rt => m_uiCell?.Rt;

        public Cell(int row, int column)
        {
            m_row = row;
            m_column = column;
        }

        #region # Set

        public Cell SetUICell(Board _, UICell uiCell)
        {
            m_uiCell = uiCell;
            return this;
        }

        public Cell SetItem(Item item)
        {
            m_item = item;
            if (m_uiCell) m_uiCell.SetItem(this, item);
            return this;
        }

        public Cell SetPositionByScreen(Vector2 screenPos, float duration = 0.1f)
        {
            if (m_uiCell) m_uiCell.SetPositionByScreen(this, screenPos, duration);
            return this;
        }

        public Cell SetActive(bool isActive)
        {
            if (m_uiCell) m_uiCell.gameObject.SetActive(isActive);
            return this;
        }

        public Cell SetEnableIcon(Board _, bool enable)
        {
            if (m_uiCell) m_uiCell.SetEnableIcon(this, enable);
            return this;
        }
        
        public Cell SetEnableFocus(Board _, bool enable)
        {
            if (m_uiCell) m_uiCell.SetEnableFocus(this, enable);
            return this;
        }

        #endregion

        public bool IsRectContains(Board _, Vector2 screenPos)
        {
            return m_uiCell != null && m_uiCell.IsRectContains(this, screenPos);
        }
    }
}