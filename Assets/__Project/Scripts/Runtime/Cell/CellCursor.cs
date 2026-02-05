using System;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class CellCursor : Cell
    {
        [SerializeField] private Cell m_selection;
        [SerializeField] private Item m_supplyItem;
        
        public Cell Selection => m_selection;
        
        public CellCursor(int row, int column) : base(row, column)
        {
            
        }

        public CellCursor SetSelection(Cell selection)
        {
            m_selection = selection;
            return this;
        }
    }
}