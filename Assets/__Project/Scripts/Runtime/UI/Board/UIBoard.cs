using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PandaIsPandaMvp
{
    public class UIBoard : MonoBehaviour
    {
        [Header("# References")] 
        [SerializeField] private RectTransform m_rt;
        [Space]
        [SerializeField] private GridLayoutGroup m_gridLayoutGroup;
        [SerializeField] private Image m_imgGridLayoutGroup;
        [Space]
        [SerializeField] private UIItem m_uiItemSelection;

        [Header("# Res")]
        [SerializeField] private UICell m_resUICell;
        
        private bool m_isCellCreated;
        
        public void Init
        (
            Cell[,] grid, 
            Cell cursor,
            Action<Cell, UICell> onCellCreated,
            Action<Cell, UICell> onCursorCreated)
        {
            // Cell 생성 ( 1 번만 실행 )
            if (!m_isCellCreated)
            {
                m_isCellCreated = true;
                
                // 매직 넘버
                const float cellSize = 124;

                // Grid 초기화
                int rowLength = grid.GetLength(0);
                int columnLength = grid.GetLength(1);

                m_gridLayoutGroup.enabled = false;
                m_gridLayoutGroup.cellSize = Vector2.one * cellSize;
                m_gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                m_gridLayoutGroup.constraintCount = columnLength;

                m_imgGridLayoutGroup.rectTransform.sizeDelta = new Vector2(rowLength * cellSize, columnLength * cellSize);

                for (int r = 0; r < rowLength; r++)
                {
                    for (int c = 0; c < columnLength; c++)
                    {
                        // 세팅으로 빼는게 정석이지만 MVP 이므로
                        Color[] colors = new Color[2]
                        {
                            new Color(1.00f, 1.00f, 1.00f, 1.00f),
                            new Color(0.97f, 0.97f, 0.97f, 1.00f),
                        };

                        // 생성, 적용 ( 이후 초기화는 Board 에게 맡김 )
                        Cell cell = grid[r, c];
                        UICell uiCell = Instantiate(m_resUICell, m_gridLayoutGroup.transform)
                            .SetBgColor(this, colors[(r + c) % 2])
                            .SetName(this, $"[ Cell ][ {r} x {c} ]");
                        
                        onCellCreated?.Invoke(cell, uiCell);
                    }
                }
                
                m_gridLayoutGroup.enabled = true;
                
                // Cursor 초기화
                UICell uiCursor = Instantiate(m_resUICell, transform)
                    .SetBgColor(this, new Color(0.0f, 0.0f, 0.0f, 0.0f))
                    .SetName(this, "[ Cell ][ Cursor ]")
                    .SetCursor(this, cellSize, m_rt);
                
                onCursorCreated?.Invoke(cursor, uiCursor);
            }

            if (m_uiItemSelection)
            {
                m_uiItemSelection
                    .SetItem(null)
                    .SetEnableIcon(false);
            }
        }

        public void SetItemSelection(Item item)
        {
            if (!m_uiItemSelection)
                return;

            m_uiItemSelection
                .SetEnableIcon(item != null)
                .SetItem(item);
        }
    }
}