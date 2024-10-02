using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {
        public class CellMove : CellComponent
        {

            public bool IsDownCellClear
            {
                get
                {
                    CellData downCell = Cell.DownCell;
                    if (downCell == null)
                    {
                        return true;
                    }
                    if (!downCell.Block.HasMoveAbleBlock)
                    {
                        return true;
                    }
                    if (downCell.Block.MoveAbleBlock.transform.localPosition.y > 0.5f)
                    {
                        return false;
                    }
                    return true;
                }
            }

            public void BlockMoveFunc()
            {
                if(SwapManager.Instance.IsContainSwapData(Cell.Pos))
                {
                    return;
                }
                if(Cell.State.IsHold)
                {
                    return;
                }
                if(!IsDownCellClear)
                {
                    return;
                }
                if(Cell.Block.HasMiddleBlock)
                {
                    return;
                }
                EmptyCellFill();
            }

            private void EmptyCellFill()
            {
                CellData upCell = Cell.UpCell;
                if(Cell.Block.HasMoveAbleBlock)
                {
                    return;
                }
                if(upCell != null && MakeNewBlock(upCell))
                {
                    return;
                }
                if(upCell != null && BlockDown(upCell))
                {
                    return;
                }
                if(IsDiagonalFill(Cell.UpRightCell, Cell.UpLeftCell))
                {
                    return;
                }
            }

            private bool MakeNewBlock(CellData upCell)
            {
                if(upCell == null)
                {
                    return false;
                }

                if(!upCell.IsGenerateCell)
                {
                    return false;
                }

                upCell.Generate.GenerateObject();
                return BlockDown(upCell);
            }

            private bool BlockDown(CellData upCell)
            {
                if(upCell == null)
                {
                    return false;
                }

                if(upCell.State.IsHold)
                {
                    return false;
                }
                if(upCell.Block.HasMoveAbleBlock && upCell.Block.MiddleBlock.State.State == BlockState.BlockStateType.Idle)
                {
                    BlockData targetBlock = upCell.Block.MiddleBlock;
                    targetBlock.RemoveCell();
                    Cell.Block.AddBlock(targetBlock, false);
                    Cell.Block.BlockMove(BlockMove.MoveType.BlockDown);
                    return true;
                }
                return false;
            }

            private bool IsDiagonalFill(CellData upRightCell, CellData upLeftCell)
            {
                if(!UpCellCheck(Cell))
                {
                    return false;
                }
                if(upLeftCell != null && upLeftCell.Block.HasMoveAbleBlock && CheckDiagonalFillConditaion(Cell, upLeftCell, true))
                {
                    return BlockDown(upLeftCell);
                }
                if(upRightCell != null && upRightCell.Block.HasMoveAbleBlock && CheckDiagonalFillConditaion(Cell, upRightCell))
                {
                    return BlockDown(upRightCell);
                }
                return false;
            }
            private bool CheckDiagonalFillConditaion(CellData pivotCell, CellData diagonalCell, bool isLeft = false)
            {
                if(OneStepUpCellDiagonalCheck(pivotCell.UpCell))
                {
                    return IsDownCellFull(diagonalCell);
                }
                else
                {
                    return IsDownCellFull(diagonalCell) && OneStepUpCellDiagonalCheck(diagonalCell.UpCell);
                }
                return false;
            }

            private bool IsDownCellFull(CellData pivotCell)
            {
                if(pivotCell == null)
                {
                    return true;
                }
                if(pivotCell.State.IsHold)
                {
                    return false;
                }
                if(pivotCell.Block.HasFixMiddleBlock)
                {
                    return true;
                }
                if(pivotCell.IsVisibleCell && !pivotCell.State.IsHold && !pivotCell.Block.HasMoveAbleBlock)
                {
                    return false;
                }
                return IsDownCellFull(pivotCell.DownCell);
            }

            private bool OneStepUpCellDiagonalCheck(CellData pivotCell, bool isLeft = false)
            {
                if(pivotCell == null)
                {
                    return true;
                }

                if(pivotCell.Block.HasMoveAbleBlock)
                {
                    return pivotCell.Block.MiddleBlock.State.State == BlockState.BlockStateType.Move;
                }

                while(pivotCell != null && pivotCell.Block.HasFixMiddleBlock)
                {
                    if(isLeft)
                    {
                        if(pivotCell.RightCell == null)
                        {
                            return true;
                        }
                        if(pivotCell.RightCell.Block.HasFixMiddleBlock)
                        {
                            return true;
                        }
                        if (pivotCell.RightCell.Block.HasMoveAbleBlock)
                        {
                            return pivotCell.RightCell.Block.MiddleBlock.State.State == BlockState.BlockStateType.Move;
                        }
                    }
                    else
                    {
                        if(pivotCell.LeftCell == null)
                        {
                            return true;
                        }
                        if(pivotCell.LeftCell.Block.HasFixMiddleBlock)
                        {
                            return true;
                        }
                        if (pivotCell.LeftCell.Block.HasMoveAbleBlock)
                        {
                            return pivotCell.LeftCell.Block.MiddleBlock.State.State == BlockState.BlockStateType.Move;
                        }
                    }
                    pivotCell = pivotCell.UpCell;
                }
                return true;
            }

            private bool OneStepUpCellCheck(CellData pivotCell)
            {
                if(pivotCell == null)
                {
                    return true;
                }

                if(pivotCell.Block.HasFixMiddleBlock)
                {
                    return true;
                }
                return false;
            }

            private bool UpCellCheck(CellData pivotCell)
            {
                if(pivotCell == null)
                {
                    return true;
                }
                if(pivotCell.Block.HasMoveAbleBlock && !pivotCell.State.IsHold)
                {
                    return false;
                }
                if(pivotCell.IsGenerateCell)
                {
                    return false;
                }
                if(pivotCell.Block.HasFixMiddleBlock)
                {
                    return true;
                }
                return UpCellCheck(pivotCell.UpCell);
            }
        }
    }
}
