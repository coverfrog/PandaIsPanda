using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PandaIsPandaMvp
{
    public class Board : MonoBehaviour
    {
        [Header("# References")]
        [SerializeField] private UIBoard m_uiBoard;

        [Header("# Res")]
        [SerializeField] private ItemConstantTable m_resItemConstantTable;

        private Cell[,] m_grid = new Cell[7, 7];
        private CellCursor m_cursor;

        private bool m_isTrack;
        private Vector2 m_inputScreenPos;

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
            m_uiBoard.Init(
                m_grid,
                m_cursor,
                onCellCreated: (cell, uiCell) =>
                {
                    cell.SetUICell(this, uiCell) // UI 부착
                        .SetItem(null) // 아이템 비우기
                        .SetEnableIcon(this, false)
                        .SetEnableFocus(this, false);
                },
                onCursorCreated: (cursor, uiCursor) =>
                {
                    cursor.SetUICell(this, uiCursor) // UI 부착
                        .SetItem(null) // 아이템 비우기
                        .SetEnableFocus(this, false)
                        .SetActive(false); // 비활성화
                });
         
            // 입력 값 수신
            InputManager inputManager = FindAnyObjectByType<InputManager>();
            
            inputManager.OnPointerClick -= OnPointerClick;
            inputManager.OnPointerClick += OnPointerClick;
            
            inputManager.OnPointerMove -= OnPointerMove;
            inputManager.OnPointerMove += OnPointerMove;
        }

        public void AddItem(bool isEnemy)
        {
            if (isEnemy)
            {
                var monster = Resources.Load<Monster>("Prefab/Monster");
                var ins = Instantiate(monster, m_uiBoard.transform);
            }
            
            else
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
        }

        public BoardData ToData()
        {
            int rowLength = m_grid.GetLength(0);
            int columnLength = m_grid.GetLength(1);

            ulong[,] grid = new ulong[rowLength, columnLength];
            
            for (int r = 0; r < rowLength; r++)
            {
                for (int c = 0; c < columnLength; c++)
                {
                    var item = m_grid[r, c].Item;
                    grid[r, c] = item != null ? item.Constant.Id : 0;
                }
            }

            var selection = m_cursor.Selection;
            
            int selectionRow = -1;
            int selectionColumn = -1;

            if (selection!= null)
            {
                selectionRow = selection.Row;
                selectionColumn = selection.Column;
            }
            
            return new BoardData()
            {
                grid = grid,
                selectionRow = selectionRow,
                selectionColumn = selectionColumn,
            };
        }

        public void LoadData(BoardData data)
        {
            var grid = data.grid;
            
            int rowLength = grid.GetLength(0);
            int columnLength = grid.GetLength(1);
            
            for (int r = 0; r < rowLength; r++)
            {
                for (int c = 0; c < columnLength; c++)
                {
                    var cell = m_grid[r, c];
                    var id = grid[r, c];
                    
                    if (id == 0)
                        continue;

                    var itemData = m_resItemConstantTable.ConstList.FirstOrDefault(x => x.Id == id);
                    cell
                        .SetItem(new Item(itemData))
                        .SetEnableIcon(this, true);
                }
            }

            if (data.selectionColumn == -1 || data.selectionRow == -1)
                return;
            
            var selected = m_grid[data.selectionRow, data.selectionColumn]
                .SetEnableFocus(this, true);

            m_cursor.SetSelection(selected);
            m_uiBoard.SetItemSelection(selected.Item);
        }
        
        private bool TryFindEmptyCell(out Cell cell)
        {
            cell = null;
            
            int rowLength = m_grid.GetLength(0);
            int columnLength = m_grid.GetLength(1);

            for (int r = 1; r < rowLength - 1; r++)
            {
                for (int c = 1; c < columnLength - 1; c++)
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

            for (int r = 1; r < rowLength - 1; r++)
            {
                for (int c = 1; c < columnLength - 1; c++)
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
                OnCellClickIn(screenPos, cell);
            }

            else
            {
                OnCellClickOut(screenPos);
            }
        }

        private void OnPointerMove(Vector2 screenPos, Vector2 delta)
        {
            if (m_cursor.Item == null)
                return;
            
            if (m_isTrack)
            {
                m_cursor.SetPositionByScreen(screenPos);
                return;
            }
            
            float distance = Vector2.Distance(m_inputScreenPos, screenPos);
            if (distance < 40.0f)
                return;

            m_isTrack = true;
            m_cursor.Selection.SetEnableFocus(this, false);
        }

        private void OnCellClickIn(Vector2 screenPos, Cell cell)
        {
            m_isTrack = false;
            m_inputScreenPos = screenPos;
            
            if (cell.Item == null)
                return;

            if (m_cursor.Selection != null &&
                m_cursor.Selection != cell)
            {
                m_cursor.Selection.SetEnableFocus(this, false);
            }
            
            m_cursor
                .SetSelection(cell)                          // 커서에 현재 선택한 Cell 저장
                .SetItem(cell.Item.DeepClone())              // 커서에 현재 아이템을 복사
                .SetPositionByScreen(RectTransformUtility.WorldToScreenPoint(null, cell.Rt.position), 0.0f) // Cell 위치로 이동 ( Cursor 인 것처럼 보이게 하기 위함 )
                .SetActive(true);                            // 커서 활성화 
            
            cell
                .SetEnableIcon(this, false) // 기존 Cell 이미지는 비 활성화
                .SetEnableFocus(this, true); 
            
            m_uiBoard.SetItemSelection(cell.Item);
        }

        private void OnCellClickOut(Vector2 screenPos)
        {
            if (m_cursor.Item == null) // 커서에 아이템 정보가 없다면 추가 동작은 없음
                return;
            
            m_cursor.SetActive(false); // 커서는 보이지 않게 처리

            if (!TryFindCell(screenPos, out Cell finedCell) ) // Cell 에 놓지 않은 경우
            {
                m_cursor
                    .SetItem(null);
                m_cursor.Selection
                    .SetEnableIcon(this, true)
                    .SetEnableFocus(this, true);
                
                return;
            }

            if (finedCell == m_cursor.Selection) // 놓은 곳에 다시 놓은 경우
            {
                m_cursor.Selection
                    .SetEnableIcon(this, true);

                m_cursor
                    .SetSelection(finedCell)
                    .SetItem(null);

                return;
            }

            if (finedCell.Item == null) // 빈 칸으로 움직인 경우
            {
                finedCell
                    .SetItem(m_cursor.Item.DeepClone())
                    .SetEnableFocus(this, true)
                    .SetEnableIcon(this, true);
                
        
                m_cursor.Selection
                    .SetItem(null)
                    .SetEnableIcon(this, false)
                    .SetEnableFocus(this, false);

                m_cursor
                    .SetSelection(finedCell)
                    .SetItem(null);
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
                        .SetEnableIcon(this, true)
                        .SetEnableFocus(this, true);

                    m_cursor.Selection
                        .SetItem(finedItem)
                        .SetEnableIcon(this, true);

                    m_cursor
                        .SetSelection(finedCell)
                        .SetItem(null);

                    return;
                }

                bool isMerge = finedCell.Item.Constant.Id == m_cursor.Item.Constant.Id && // 동일한 아이템 Constant 이고
                               finedCell.Item.Constant.MergedId != 0;                     // 최종 아이템이 아니라면

                if (isMerge)
                {
                    // 다음에 해당하는 아이템 찾기
                    ItemConstant next = m_resItemConstantTable.ConstList.FirstOrDefault(x => x.Id == finedCell.Item.Constant.MergedId);
                    Item nextItem = new Item(next);
                    
                    // 신규 아이템 적용
                    finedCell.SetItem(nextItem)
                        .SetEnableIcon(this, true)
                        .SetEnableFocus(this, true);
                    
                    // 기존 Cell은 비우기, 새롭게 Merge 된 Item을 선택
                    m_cursor.Selection
                        .SetItem(null)
                        .SetEnableIcon(this, false);
                    
                    m_cursor
                        .SetSelection(finedCell)
                        .SetItem(null);
                    
                    m_uiBoard.SetItemSelection(nextItem);
                }
            }
        }
    }
}