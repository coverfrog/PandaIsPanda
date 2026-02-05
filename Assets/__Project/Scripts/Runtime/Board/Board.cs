using System.Linq;
using UnityEngine;

namespace PandaIsPanda
{
    public class Board : MonoBehaviour
    {
        [Header("# Res")]
        [SerializeField] private ItemConstantTable m_resItemConstantTable;

        private UIBoard m_uiBoard;
        
        private Cell[,] m_grid = new Cell[9, 7];
        private CellCursor m_cursor;
        
        public void Init()
        {
            // Grid 초기화
            int rowLength = m_grid.GetLength(0);
            int columnLength = m_grid.GetLength(1);

            for (int r = 0; r < rowLength; r++)
            {
                for (int c = 0; c < columnLength; c++)
                {
                    m_grid[r, c] = new Cell(r, c);
                }
            }
            
            // Cursor 초기화
            m_cursor = new CellCursor(-1, -1);
            
            // 보드 Cell UI 초기화
            m_uiBoard = FindAnyObjectByType<UIBoard>();
            m_uiBoard.Init(
                m_grid,
                m_cursor,
                onCellCreated: (cell, uiCell) =>
                {
                    cell.SetUICell(this, uiCell) // UI 부착
                        .SetItem(null) // 아이템 비우기
                        .SetEnableIcon(this, false);
                },
                onCursorCreated: (cursor, uiCursor) =>
                {
                    cursor.SetUICell(this, uiCursor) // UI 부착
                        .SetItem(null) // 아이템 비우기
                        .SetActive(false); // 비활성화
                });
            
            // 0 번째 인덱스에 아이템 배치
            AddItem();
            
            // 입력 값 수신
            InputManager inputManager = FindAnyObjectByType<InputManager>();
            
            inputManager.OnPointerClick -= OnPointerClick;
            inputManager.OnPointerClick += OnPointerClick;
            
            inputManager.OnPointerMove -= OnPointerMove;
            inputManager.OnPointerMove += OnPointerMove;
        }

        public void AddItem()
        {
            if (!TryFindEmptyCell(out Cell emptyCell)) 
                return;

            var constants = m_resItemConstantTable.ConstList.Where(x => x.Grade == 1).ToList();
            
            int count = constants.Count;
            
            ItemConstant constant = constants[Random.Range(0, count)];
            Item item = new Item(constant);

            emptyCell
                .SetItem(item)
                .SetEnableIcon(this, true);
        }

        private bool TryFindEmptyCell(out Cell cell)
        {
            cell = null;
            
            int rowLength = m_grid.GetLength(0);
            int columnLength = m_grid.GetLength(1);

            for (int r = 0; r < rowLength; r++)
            {
                for (int c = 0; c < columnLength; c++)
                {
                    if (m_grid[r, c].Item != null)
                        continue;
                    
                    cell = m_grid[r, c];
                    return true;
                }
            }
            
            return false;
        }

        private bool TryFindCell(Vector2 screenPos, out Cell cell)
        {
            cell = null;
            
            int rowLength = m_grid.GetLength(0);
            int columnLength = m_grid.GetLength(1);

            for (int r = 0; r < rowLength; r++)
            {
                for (int c = 0; c < columnLength; c++)
                {
                    cell = m_grid[r, c];
                    
                    if (cell.IsRectContains(this, screenPos))
                        return true;
                }
            }
            
            return false;
        }

        private void OnPointerClick(bool isClick, Vector2 screenPos)
        {
            if (isClick && TryFindCell(screenPos, out Cell cell))
            {
                OnCellClickIn(cell);
            }

            else
            {
                OnCellClickOut(screenPos);
            }
        }

        private void OnPointerMove(Vector2 screenPos, Vector2 delta)
        {
            m_cursor.SetAnchorPosition(screenPos);
        }

        private void OnCellClickIn(Cell cell)
        {
            if (cell.Item == null)
                return;

            m_cursor
                .SetSelection(cell)                  // 커서에 현재 선택한 Cell 저장
                .SetItem(cell.Item.DeepClone())      // 커서에 현재 아이템을 복사
                .SetActive(true);                    // 커서 활성화 
            
            cell.SetEnableIcon(this, false); // 기존 Cell 이미지는 비 활성화
        }

        private void OnCellClickOut(Vector2 screenPos)
        {
            if (m_cursor.Item == null) // 커서에 아이템 정보가 없다면 추가 동작은 없음
                return;
            m_cursor.SetActive(false); // 커서는 보이지 않게 처리

            if (!TryFindCell(screenPos, out Cell finedCell) ) // Cell 에 놓지 않은 경우
            {
                m_cursor.SetItem(null);
                m_cursor.Selection.SetEnableIcon(this, true);
                
                return;
            }

            if (finedCell == m_cursor.Selection) // 놓은 곳에 다시 놓은 경우
            {
                m_cursor.SetItem(null);
                m_cursor.Selection.SetEnableIcon(this, true);

                return;
            }

            if (finedCell.Item == null) // 빈 칸으로 움직인 경우
            {
                finedCell.SetItem(m_cursor.Item.DeepClone())
                         .SetEnableIcon(this, true);
                
                m_cursor
                    .SetItem(null);
                m_cursor.Selection
                    .SetItem(null)
                    .SetEnableIcon(this, false);
            }
            
            else // 아이템은 존재
            {
                bool isSwap = finedCell.Item.Constant.Id == m_cursor.Item.Constant.Id && // 동잉 아이템이지만
                              finedCell.Item.Constant.MergedId == 0;                     // 이미 최종 아이템인 경우
                
                if (!isSwap) 
                    isSwap = finedCell.Item.Constant.Id != m_cursor.Item.Constant.Id;    // 서로 다른 아이템인 경우 

                if (isSwap)
                {
                    // 복사
                    var cursorItem = m_cursor.Item.DeepClone();
                    var finedItem = finedCell.Item.DeepClone();
                    
                    // 서로 다른 아이템으로 처리
                    finedCell
                        .SetItem(cursorItem)
                        .SetEnableIcon(this, true);

                    m_cursor.Selection
                        .SetItem(finedItem)
                        .SetEnableIcon(this, true);
                    m_cursor.SetItem(null);

                    return;
                }

                bool isMerge = finedCell.Item.Constant.Id == m_cursor.Item.Constant.Id && // 동일한 아이템 Constant 이고
                               finedCell.Item.Constant.MergedId != 0;                     // 최종 아이템이 아니라면

                if (isMerge)
                {
                    // 다음에 해당하는 아이템 찾기
                    ItemConstant next = m_resItemConstantTable.ConstList.FirstOrDefault(x => x.Id == finedCell.Item.Constant.MergedId);
                    Item nextItem = new Item(next);
                    
                    // 기존 Cell은 비우기
                    m_cursor.Selection
                        .SetItem(null)
                        .SetEnableIcon(this, false);
                    m_cursor.SetItem(null);
                    
                    // 신규 아이템 적용
                    finedCell.SetItem(nextItem)
                        .SetEnableIcon(this, true);
                }
            }
        }
    }
}