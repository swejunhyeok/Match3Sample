using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {
        public class GridData : Match3Data
        {

            #region Grid data

            private Transform _trGrid;
            public Transform TrGrid
            {
                get
                {
                    if(_trGrid == null)
                    {
                        _trGrid = transform;
                    }
                    return _trGrid;
                }
            }

            #endregion

            #region Cell data

            private CellData[,] _cells = new CellData[(ConstantData.MAX_GRID_HEIGHT_SIZE + 1), ConstantData.MAX_GRID_WIDTH_SIZE];
            
            public CellData GetCell(Vector2Int pos)
            {
                return GetCell(pos.x, pos.y);
            }
            
            public CellData GetCell(int x, int y)
            {
                if(_cells == null)
                {
                    return null;
                }
                if(x < 0 || x >= _cells.GetLength(1))
                {
                    return null;
                }
                if(y < 0 || y >= _cells.GetLength(0))
                {
                    return null;
                }
                return _cells[y, x];
            }

            public void SetArroundCell()
            {
                for (int y = 0; y < ConstantData.MAX_GRID_HEIGHT_SIZE; ++y)
                {
                    for (int x = 0; x < ConstantData.MAX_GRID_WIDTH_SIZE; ++x)
                    {
                        Vector2Int pivotPos = new Vector2Int(x, y);
                        Vector2Int movePos;
                        CellData targetCell;

                        CellData[] fourDirectionCell = new CellData[4];
                        for(int i = 0; i < CellIndex.FourDirection.Length; ++i)
                        {
                            movePos = pivotPos + CellIndex.FourDirection[i];
                            targetCell = GetCell(movePos);
                            if(CellIndex.Verification(movePos, true) && targetCell != null && targetCell.IsWorkCell)
                            {
                                fourDirectionCell[i] = targetCell;
                            }
                        }

                        CellData[] fourDiagonalCell = new CellData[4];
                        for(int i = 0; i < CellIndex.FourDiagonalDirection.Length; ++i)
                        {
                            movePos = pivotPos + CellIndex.FourDiagonalDirection[i];
                            targetCell = GetCell(movePos);
                            if (CellIndex.Verification(movePos, true) && targetCell != null && targetCell.IsWorkCell)
                            {
                                fourDirectionCell[i] = targetCell;
                            }
                        }

                        GetCell(pivotPos).SetArroundCell(fourDirectionCell, fourDiagonalCell);
                    }
                }
            }

            #endregion

            #region Data load

            public void LoadGridData(LitJson.JsonData gridRoot)
            {
                LitJson.JsonData cellsRoot = gridRoot[ConstantData.MAP_KEY_CELL_LIST];

                for(int i = 0; i < cellsRoot.Count; ++i)
                {
                    LitJson.JsonData cellRoot = cellsRoot[i];
                    CellData cell = ObjectPoolManager.Instance.GetCellData(TrGrid);
                    Vector2Int pos = CellIndex.IndexConvertToPos(i);

                    cell.transform.localPosition = (Vector3Int)pos + new Vector3(-4f, -5f);
                    cell.LoadCellData(cellRoot, pos, i);
                    _cells[pos.y, pos.x] = cell;
                }

                for(int i = 0; i <cellsRoot.Count; ++i)
                {
                    LitJson.JsonData cellRoot = cellsRoot[i];
                    Vector2Int pos = CellIndex.IndexConvertToPos(i);
                    _cells[pos.y, pos.x].LoadBlockData(cellRoot);
                }

                SetArroundCell();
            }


            #endregion

            #region Input

            public bool VerificationSwap(Vector2Int pivotPos, Vector2Int direction)
            {
                if(!CellIndex.Verification(pivotPos))
                {
                    return false;
                }

                if(!CellIndex.Verification(pivotPos, direction))
                {
                    return false;
                }

                if(!GetCell(pivotPos).Block.IsSwapAble)
                {
                    return false;
                }

                if(!GetCell(pivotPos + direction).Block.IsSwapAble && !GetCell(pivotPos + direction).Block.IsEmpty)
                {
                    return false;
                }

                if(!GetCell(pivotPos).Block.HasMoveAbleBlock && !GetCell(pivotPos + direction).Block.HasMoveAbleBlock)
                {
                    return false;
                }

                if(GetCell(pivotPos).Block.IsEmpty && GetCell(pivotPos + direction).Block.IsEmpty)
                {
                    return false;
                }

                return true;
            }

            public void InputSwap(Vector2Int pivotPos, Vector2Int direction)
            {
                CellData pivotCell = GetCell(pivotPos);
                CellData targetCell = GetCell(pivotPos + direction);

                if(pivotCell.Block.HasSwapAbleBlock)
                {
                    GameManager.Instance.AddWaitingSwapNum();
                    pivotCell.State.AddHoldState();
                    targetCell.State.AddHoldState();
                    pivotCell.Block.SwapAbleBlock.Cache.MoveEndAction += () =>
                    {
                        GameManager.Instance.ReduceWaitingSwapNum();
                        pivotCell.State.ReduceHoldState();
                        targetCell.State.ReduceHoldState();
                    };
                }
                if(targetCell.Block.HasSwapAbleBlock)
                {
                    GameManager.Instance.AddWaitingSwapNum();
                    pivotCell.State.AddHoldState();
                    targetCell.State.AddHoldState();
                    targetCell.Block.SwapAbleBlock.Cache.MoveEndAction += () =>
                    {
                        GameManager.Instance.ReduceWaitingSwapNum();
                        pivotCell.State.ReduceHoldState();
                        targetCell.State.ReduceHoldState();
                    };
                }


            }

            #endregion

        }
    }
}
