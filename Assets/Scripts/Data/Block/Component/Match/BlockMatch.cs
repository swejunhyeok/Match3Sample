using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {
        public class BlockMatch : BlockComponent
        {

            public enum BlockMatchType
            {
                None,
                Three,
                Prop,
                RocketTransverse,
                RocketVertical,
                Bomb,
                Rainbow
            }

            public struct MatchInfo
            {
                public CellData PivotCell;
                public int SameDirectionIndex;
                public int[] ReverseDirectionIndex;
            }

            public struct MatchData
            {
                public BlockMatchType Type;
                public CellData PivotCell;
                public List<MatchInfo> MatchInfos;
            }

            private void FindSameColorBlock(List<CellData> verticalMatchCell, List<CellData> horizontalMatchCell, bool isSwap, out int[] lastMatchCellIndex )
            {
                lastMatchCellIndex = new int[4] { -1, -1, -1, -1 };
                if(Block.PivotCell)
                {
                    return;
                }

                for(int i = 0; i < CellIndex.FourDirection.Length; ++i)
                {
                    CellData targetCell = Block.PivotCell.FourDirectionCell[i];
                    for(int j = 0; j < ConstantData.MATCH_SEARCH_SPACE; ++j)
                    {
                        if(targetCell == null || !targetCell.IsVisibleCell)
                        {
                            break;
                        }
                        if(!IsSameColorBlock(targetCell, isSwap))
                        {
                            break;
                        }

                        if(i == CellIndex.UpIndex || i == CellIndex.DownIndex)
                        {
                            verticalMatchCell.Add(targetCell);
                        }
                        else
                        {
                            horizontalMatchCell.Add(targetCell);
                        }
                        lastMatchCellIndex[i] = targetCell.Index;
                        targetCell = targetCell.FourDirectionCell[i];
                    }
                }
            }
            private void FindSameColorBlock(bool isSwap, out int verticalSameNum, out int horizontalSameNum)
            {
                verticalSameNum = 0;
                horizontalSameNum = 0;
                if(GameManager.Instance.IsExistRunningRainbowEffect((int)Block.Attribute.Color))
                {
                    return;
                }
                for(int i = 0; i < CellIndex.FourDirection.Length; ++i)
                {
                    CellData targetCell = Block.PivotCell.FourDirectionCell[i];
                    for(int j = 0; j < ConstantData.MATCH_SEARCH_SPACE; ++j)
                    {
                        if(targetCell == null || !targetCell.IsVisibleCell)
                        {
                            break;
                        }
                        if(!IsSameColorBlock(targetCell, isSwap))
                        {
                            break;
                        }
                        if(i == CellIndex.UpIndex || i == CellIndex.DownIndex)
                        {
                            ++verticalSameNum;
                        }
                        else
                        {
                            ++horizontalSameNum;
                        }
                        targetCell = targetCell.FourDirectionCell[i];
                    }
                }
            }

            public int MatchPreprocessing(bool isSwap = false)
            {
                FindSameColorBlock(isSwap, out int verticalSameNum, out int horizontalSameNum);
                if (verticalSameNum == 4 || horizontalSameNum == 4)
                {
                    return 5;
                }
                else if (verticalSameNum >= 2 && horizontalSameNum >= 2)
                {
                    return 4;
                }
                else if (verticalSameNum == 3 || horizontalSameNum == 3)
                {
                    return 3;
                }
                else if (verticalSameNum >= 1 && horizontalSameNum >= 1)
                {
                    return 2;
                }
                else if (verticalSameNum == 2 || horizontalSameNum == 2)
                {
                    return 1;
                }
                return 0;
            }

            public MatchData MatchSearch(BlockMatchType type, bool isSwap = false)
            {
                if(Block.State.State == BlockState.BlockStateType.Match || Block.State.State == BlockState.BlockStateType.Move)
                {
                    return new MatchData() { Type = BlockMatchType.None };
                }

                List<CellData> verticalMatchCell = new List<CellData>();
                List<CellData> horizontalMatchCell = new List<CellData>();
                List<CellData> propCell = new List<CellData>();
                CellData targetCell = null;

                int[] lastMatchCellIndex = new int[4] { -1, -1, -1, -1 };
                int propDirection = -1;

                if(type == BlockMatchType.Prop)
                {
                    bool[] isSameColorExist = new bool[4] { false, false, false, false };
                    
                    for(int i = 0; i < CellIndex.FourDirection.Length; ++i)
                    {
                        targetCell = Block.PivotCell.FourDirectionCell[i];
                        if (targetCell == null || !targetCell.IsVisibleCell || !IsSameColorBlock(targetCell, isSwap))
                        {
                            continue;
                        }
                        isSameColorExist[i] = true;
                    }

                    if (isSameColorExist[CellIndex.UpIndex] && (isSameColorExist[CellIndex.LeftIndex] || isSameColorExist[CellIndex.RightIndex]))
                    {
                        if (isSameColorExist[CellIndex.LeftIndex] && IsSameColorBlock(Block.PivotCell.UpLeftCell, isSwap))
                        {
                            propCell.Add(Block.PivotCell.UpCell);
                            propCell.Add(Block.PivotCell.LeftCell);
                            propCell.Add(Block.PivotCell.UpLeftCell);
                            propDirection = CellIndex.UpLeftIndex;
                        }
                        else if (isSameColorExist[CellIndex.RightIndex] && IsSameColorBlock(Block.PivotCell.UpRightCell, isSwap))
                        {
                            propCell.Add(Block.PivotCell.UpCell);
                            propCell.Add(Block.PivotCell.RightCell);
                            propCell.Add(Block.PivotCell.UpRightCell);
                            propDirection = CellIndex.UpRightIndex;
                        }
                    }
                    if (isSameColorExist[CellIndex.DownIndex] && (isSameColorExist[CellIndex.LeftIndex] || isSameColorExist[CellIndex.RightIndex]))
                    {
                        if (isSameColorExist[CellIndex.LeftIndex] && IsSameColorBlock(Block.PivotCell.DownLeftCell, isSwap))
                        {
                            propCell.Add(Block.PivotCell.DownCell);
                            propCell.Add(Block.PivotCell.LeftCell);
                            propCell.Add(Block.PivotCell.DownLeftCell);
                            propDirection = CellIndex.DownLeftIndex;
                        }
                        else if (isSameColorExist[CellIndex.RightIndex] && IsSameColorBlock(Block.PivotCell.DownRightCell, isSwap))
                        {
                            propCell.Add(Block.PivotCell.DownCell);
                            propCell.Add(Block.PivotCell.RightCell);
                            propCell.Add(Block.PivotCell.DownRightCell);
                            propDirection = CellIndex.DownRightIndex;
                        }
                    }
                }
                else
                {
                    FindSameColorBlock(verticalMatchCell, horizontalMatchCell, isSwap, out lastMatchCellIndex);
                }

                switch(type)
                {
                    case BlockMatchType.Rainbow:
                    {
                        if (verticalMatchCell.Count >= 4)
                        {
                            List<MatchInfo> matchInfos = new List<MatchInfo>();
                            matchInfos.Add(new MatchInfo() 
                            { 
                                PivotCell = Block.PivotCell, 
                                SameDirectionIndex = -1, 
                                ReverseDirectionIndex = new int[] { CellIndex.LeftIndex, CellIndex.RightIndex } 
                            });
                            Block.State.SetState(BlockState.BlockStateType.Match);
                            for (int i = 0; i < verticalMatchCell.Count; ++i)
                            {
                                verticalMatchCell[i].Block.MiddleBlock.State.SetState(BlockState.BlockStateType.Match);
                                if (verticalMatchCell[i].Index == lastMatchCellIndex[CellIndex.UpIndex])
                                {
                                    matchInfos.Add(new MatchInfo()
                                    {
                                        PivotCell = verticalMatchCell[i],
                                        SameDirectionIndex = CellIndex.UpIndex,
                                        ReverseDirectionIndex = new int[] { CellIndex.LeftIndex, CellIndex.RightIndex }
                                    });
                                }
                                else if (verticalMatchCell[i].Index == lastMatchCellIndex[CellIndex.DownIndex])
                                {
                                    matchInfos.Add(new MatchInfo()
                                    {
                                        PivotCell = verticalMatchCell[i],
                                        SameDirectionIndex = CellIndex.DownIndex,
                                        ReverseDirectionIndex = new int[] { CellIndex.LeftIndex, CellIndex.RightIndex }
                                    });
                                }
                                else
                                {
                                    matchInfos.Add(new MatchInfo()
                                    {
                                        PivotCell = verticalMatchCell[i],
                                        SameDirectionIndex = -1,
                                        ReverseDirectionIndex = new int[] { CellIndex.LeftIndex, CellIndex.RightIndex }
                                    });
                                }
                            }
                            return new MatchData() { Type = type, PivotCell = Block.PivotCell, MatchInfos = matchInfos };
                        }
                        if (horizontalMatchCell.Count >= 4)
                        {
                            List<MatchInfo> matchInfos = new List<MatchInfo>();
                            matchInfos.Add(new MatchInfo()
                            {
                                PivotCell = Block.PivotCell,
                                SameDirectionIndex = -1,
                                ReverseDirectionIndex = new int[] { CellIndex.UpIndex, CellIndex.DownIndex }
                            });
                            Block.State.SetState(BlockState.BlockStateType.Match);
                            for(int i = 0; i < horizontalMatchCell.Count; ++i)
                            {
                                horizontalMatchCell[i].Block.MiddleBlock.State.SetState(BlockState.BlockStateType.Match);
                                if (horizontalMatchCell[i].Index == lastMatchCellIndex[CellIndex.LeftIndex])
                                {
                                    matchInfos.Add(new MatchInfo()
                                    {
                                        PivotCell = horizontalMatchCell[i],
                                        SameDirectionIndex = CellIndex.LeftIndex,
                                        ReverseDirectionIndex = new int[] { CellIndex.UpIndex, CellIndex.DownIndex }
                                    });
                                }
                                else if (horizontalMatchCell[i].Index == lastMatchCellIndex[CellIndex.RightIndex])
                                {
                                    matchInfos.Add(new MatchInfo()
                                    {
                                        PivotCell = horizontalMatchCell[i],
                                        SameDirectionIndex = CellIndex.RightIndex,
                                        ReverseDirectionIndex = new int[] { CellIndex.UpIndex, CellIndex.DownIndex }
                                    });
                                }
                                else
                                {
                                    matchInfos.Add(new MatchInfo()
                                    {
                                        PivotCell = horizontalMatchCell[i],
                                        SameDirectionIndex = -1,
                                        ReverseDirectionIndex = new int[] { CellIndex.UpIndex, CellIndex.DownIndex }
                                    });
                                }
                            }
                            return new MatchData() { Type = type, PivotCell = Block.PivotCell, MatchInfos = matchInfos };
                        }
                        break;
                    }
                    case BlockMatchType.Bomb:
                    {
                        if(verticalMatchCell.Count >= 2 && horizontalMatchCell.Count >= 2)
                        {
                            List<MatchInfo> matchInfos = new List<MatchInfo>();
                            matchInfos.Add(new MatchInfo()
                            {
                                PivotCell = Block.PivotCell,
                                SameDirectionIndex = -1,
                                ReverseDirectionIndex = null
                            });
                            Block.State.SetState(BlockState.BlockStateType.Match);
                            for(int i = 0; i < verticalMatchCell.Count; ++i)
                            {
                                verticalMatchCell[i].Block.MiddleBlock.State.SetState(BlockState.BlockStateType.Match);
                                if (verticalMatchCell[i].Index == lastMatchCellIndex[CellIndex.UpIndex])
                                {
                                    matchInfos.Add(new MatchInfo()
                                    {
                                        PivotCell = verticalMatchCell[i],
                                        SameDirectionIndex = CellIndex.UpIndex,
                                        ReverseDirectionIndex = new int[] { CellIndex.LeftIndex, CellIndex.RightIndex }
                                    });
                                }
                                else if (verticalMatchCell[i].Index == lastMatchCellIndex[CellIndex.DownIndex])
                                {
                                    matchInfos.Add(new MatchInfo()
                                    {
                                        PivotCell = verticalMatchCell[i],
                                        SameDirectionIndex = CellIndex.DownIndex,
                                        ReverseDirectionIndex = new int[] { CellIndex.LeftIndex, CellIndex.RightIndex }
                                    });
                                }
                                else
                                {
                                    matchInfos.Add(new MatchInfo()
                                    {
                                        PivotCell = verticalMatchCell[i],
                                        SameDirectionIndex = -1,
                                        ReverseDirectionIndex = new int[] { CellIndex.LeftIndex, CellIndex.RightIndex }
                                    });
                                }
                            }
                            for(int i = 0; i < horizontalMatchCell.Count; ++i)
                            {
                                horizontalMatchCell[i].Block.MiddleBlock.State.SetState(BlockState.BlockStateType.Match);
                                if (horizontalMatchCell[i].Index == lastMatchCellIndex[CellIndex.LeftIndex])
                                {
                                    matchInfos.Add(new MatchInfo()
                                    {
                                        PivotCell = horizontalMatchCell[i],
                                        SameDirectionIndex = CellIndex.LeftIndex,
                                        ReverseDirectionIndex = new int[] { CellIndex.UpIndex, CellIndex.DownIndex }
                                    });
                                }
                                else if (horizontalMatchCell[i].Index == lastMatchCellIndex[CellIndex.RightIndex])
                                {
                                    matchInfos.Add(new MatchInfo()
                                    {
                                        PivotCell = horizontalMatchCell[i],
                                        SameDirectionIndex = CellIndex.RightIndex,
                                        ReverseDirectionIndex = new int[] { CellIndex.UpIndex, CellIndex.DownIndex }
                                    });
                                }
                                else
                                {
                                    matchInfos.Add(new MatchInfo()
                                    {
                                        PivotCell = horizontalMatchCell[i],
                                        SameDirectionIndex = -1,
                                        ReverseDirectionIndex = new int[] { CellIndex.UpIndex, CellIndex.DownIndex }
                                    });
                                }
                            }
                            return new MatchData() { Type = type, PivotCell = Block.PivotCell, MatchInfos = matchInfos };
                        }
                        break;
                    }
                    case BlockMatchType.RocketTransverse:
                    {
                        if(verticalMatchCell.Count >= 3)
                        {
                            List<MatchInfo> matchInfos = new List<MatchInfo>();
                            matchInfos.Add(new MatchInfo()
                            {
                                PivotCell = Block.PivotCell,
                                SameDirectionIndex = -1,
                                ReverseDirectionIndex = null
                            });
                            Block.State.SetState(BlockState.BlockStateType.Match);
                            for (int i = 0; i < verticalMatchCell.Count; ++i)
                            {
                                verticalMatchCell[i].Block.MiddleBlock.State.SetState(BlockState.BlockStateType.Match);
                                matchInfos.Add(new MatchInfo()
                                {
                                    PivotCell = verticalMatchCell[i],
                                    SameDirectionIndex = -1,
                                    ReverseDirectionIndex = null
                                });
                            }
                            return new MatchData() { Type = type, PivotCell = Block.PivotCell, MatchInfos = matchInfos };
                        }
                        break;
                    }
                    case BlockMatchType.RocketVertical:
                    {
                        if(horizontalMatchCell.Count >= 3)
                        {
                            List<MatchInfo> matchInfos = new List<MatchInfo>();
                            matchInfos.Add(new MatchInfo()
                            {
                                PivotCell = Block.PivotCell,
                                SameDirectionIndex = -1,
                                ReverseDirectionIndex = null
                            });
                            Block.State.SetState(BlockState.BlockStateType.Match);
                            for (int i = 0; i < horizontalMatchCell.Count; ++i)
                            {
                                horizontalMatchCell[i].Block.MiddleBlock.State.SetState(BlockState.BlockStateType.Match);
                                matchInfos.Add(new MatchInfo()
                                {
                                    PivotCell = horizontalMatchCell[i],
                                    SameDirectionIndex = -1,
                                    ReverseDirectionIndex = null
                                });
                            }
                            return new MatchData() { Type = type, PivotCell = Block.PivotCell, MatchInfos = matchInfos };
                        }
                        break;
                    }
                    case BlockMatchType.Prop:
                    {
                        if(propCell.Count >= 3)
                        {
                            List<MatchInfo> matchInfos = new List<MatchInfo>();
                            if(propDirection == CellIndex.UpLeftIndex)
                            {
                                matchInfos.Add(new MatchInfo()
                                {
                                    PivotCell = Block.PivotCell,
                                    SameDirectionIndex = -1,
                                    ReverseDirectionIndex = new int[] {CellIndex.DownIndex, CellIndex.RightIndex}
                                });
                            }
                            else if(propDirection == CellIndex.UpRightIndex)
                            {
                                matchInfos.Add(new MatchInfo()
                                {
                                    PivotCell = Block.PivotCell,
                                    SameDirectionIndex = -1,
                                    ReverseDirectionIndex = new int[] { CellIndex.DownIndex, CellIndex.LeftIndex }
                                });
                            }
                            else if(propDirection == CellIndex.DownLeftIndex)
                            {
                                matchInfos.Add(new MatchInfo()
                                {
                                    PivotCell = Block.PivotCell,
                                    SameDirectionIndex = -1,
                                    ReverseDirectionIndex = new int[] { CellIndex.UpIndex, CellIndex.RightIndex }
                                });
                            }
                            else if(propDirection == CellIndex.DownRightIndex)
                            {
                                matchInfos.Add(new MatchInfo()
                                {
                                    PivotCell = Block.PivotCell,
                                    SameDirectionIndex = -1,
                                    ReverseDirectionIndex = new int[] { CellIndex.UpIndex, CellIndex.LeftIndex }
                                });
                            }
                            Block.State.SetState(BlockState.BlockStateType.Match);

                            for(int i = 0; i < propCell.Count; ++i)
                            {
                                propCell[i].Block.MiddleBlock.State.SetState(BlockState.BlockStateType.Match);
                                if(i == 0)
                                {
                                    if(propDirection == CellIndex.UpLeftIndex)
                                    {
                                        matchInfos.Add(new MatchInfo()
                                        {
                                            PivotCell = propCell[i],
                                            SameDirectionIndex = -1,
                                            ReverseDirectionIndex = new int[] { CellIndex.UpIndex, CellIndex.RightIndex }
                                        });
                                    }
                                    else if(propDirection == CellIndex.UpRightIndex)
                                    {
                                        matchInfos.Add(new MatchInfo()
                                        {
                                            PivotCell = propCell[i],
                                            SameDirectionIndex = -1,
                                            ReverseDirectionIndex = new int[] { CellIndex.UpIndex, CellIndex.LeftIndex }
                                        });
                                    }
                                    else if(propDirection == CellIndex.DownLeftIndex)
                                    {
                                        matchInfos.Add(new MatchInfo()
                                        {
                                            PivotCell = propCell[i],
                                            SameDirectionIndex = -1,
                                            ReverseDirectionIndex = new int[] { CellIndex.DownIndex, CellIndex.RightIndex }
                                        });
                                    }
                                    else if(propDirection == CellIndex.DownRightIndex)
                                    {
                                        matchInfos.Add(new MatchInfo()
                                        {
                                            PivotCell = propCell[i],
                                            SameDirectionIndex = -1,
                                            ReverseDirectionIndex = new int[] { CellIndex.DownIndex, CellIndex.LeftIndex }
                                        });
                                    }
                                }
                                else if(i == 1)
                                {
                                    if (propDirection == CellIndex.UpLeftIndex)
                                    {
                                        matchInfos.Add(new MatchInfo()
                                        {
                                            PivotCell = propCell[i],
                                            SameDirectionIndex = -1,
                                            ReverseDirectionIndex = new int[] { CellIndex.DownIndex, CellIndex.LeftIndex }
                                        });
                                    }
                                    else if (propDirection == CellIndex.UpRightIndex)
                                    {
                                        matchInfos.Add(new MatchInfo()
                                        {
                                            PivotCell = propCell[i],
                                            SameDirectionIndex = -1,
                                            ReverseDirectionIndex = new int[] { CellIndex.DownIndex, CellIndex.RightIndex }
                                        });
                                    }
                                    else if (propDirection == CellIndex.DownLeftIndex)
                                    {
                                        matchInfos.Add(new MatchInfo()
                                        {
                                            PivotCell = propCell[i],
                                            SameDirectionIndex = -1,
                                            ReverseDirectionIndex = new int[] { CellIndex.UpIndex, CellIndex.LeftIndex }
                                        });
                                    }
                                    else if (propDirection == CellIndex.DownRightIndex)
                                    {
                                        matchInfos.Add(new MatchInfo()
                                        {
                                            PivotCell = propCell[i],
                                            SameDirectionIndex = -1,
                                            ReverseDirectionIndex = new int[] { CellIndex.UpIndex, CellIndex.RightIndex }
                                        });
                                    }
                                }
                                else if(i == 2)
                                {
                                    if (propDirection == CellIndex.UpLeftIndex)
                                    {
                                        matchInfos.Add(new MatchInfo()
                                        {
                                            PivotCell = propCell[i],
                                            SameDirectionIndex = -1,
                                            ReverseDirectionIndex = new int[] { CellIndex.UpIndex, CellIndex.LeftIndex }
                                        });
                                    }
                                    else if (propDirection == CellIndex.UpRightIndex)
                                    {
                                        matchInfos.Add(new MatchInfo()
                                        {
                                            PivotCell = propCell[i],
                                            SameDirectionIndex = -1,
                                            ReverseDirectionIndex = new int[] { CellIndex.UpIndex, CellIndex.RightIndex }
                                        });
                                    }
                                    else if (propDirection == CellIndex.DownLeftIndex)
                                    {
                                        matchInfos.Add(new MatchInfo()
                                        {
                                            PivotCell = propCell[i],
                                            SameDirectionIndex = -1,
                                            ReverseDirectionIndex = new int[] { CellIndex.DownIndex, CellIndex.LeftIndex }
                                        });
                                    }
                                    else if (propDirection == CellIndex.DownRightIndex)
                                    {
                                        matchInfos.Add(new MatchInfo()
                                        {
                                            PivotCell = propCell[i],
                                            SameDirectionIndex = -1,
                                            ReverseDirectionIndex = new int[] { CellIndex.DownIndex, CellIndex.RightIndex }
                                        });
                                    }
                                }
                            }
                            return new MatchData() { Type = type, PivotCell = Block.PivotCell, MatchInfos = matchInfos };
                        }
                        break;
                    }
                    case BlockMatchType.Three:
                    {
                        if (verticalMatchCell.Count >= 2)
                        {
                            List<MatchInfo> matchInfos = new List<MatchInfo>();
                            matchInfos.Add(new MatchInfo()
                            {
                                PivotCell = Block.PivotCell,
                                SameDirectionIndex = -1,
                                ReverseDirectionIndex = null
                            });
                            Block.State.SetState(BlockState.BlockStateType.Match);
                            for (int i = 0; i < verticalMatchCell.Count; ++i)
                            {
                                verticalMatchCell[i].Block.MiddleBlock.State.SetState(BlockState.BlockStateType.Match);
                                matchInfos.Add(new MatchInfo()
                                {
                                    PivotCell = verticalMatchCell[i],
                                    SameDirectionIndex = -1,
                                    ReverseDirectionIndex = null
                                });
                            }
                            return new MatchData() { Type = type, PivotCell = Block.PivotCell, MatchInfos = matchInfos };
                        }
                        if (horizontalMatchCell.Count >= 2)
                        {
                            List<MatchInfo> matchInfos = new List<MatchInfo>();
                            matchInfos.Add(new MatchInfo()
                            {
                                PivotCell = Block.PivotCell,
                                SameDirectionIndex = -1,
                                ReverseDirectionIndex = null
                            });
                            Block.State.SetState(BlockState.BlockStateType.Match);
                            for (int i = 0; i < horizontalMatchCell.Count; ++i)
                            {
                                horizontalMatchCell[i].Block.MiddleBlock.State.SetState(BlockState.BlockStateType.Match);
                                matchInfos.Add(new MatchInfo()
                                {
                                    PivotCell = horizontalMatchCell[i],
                                    SameDirectionIndex = -1,
                                    ReverseDirectionIndex = null
                                });
                            }
                            return new MatchData() { Type = type, PivotCell = Block.PivotCell, MatchInfos = matchInfos };
                        }
                        break;
                    }
                }
                return new MatchData() { Type = BlockMatchType.None };
            }

            public bool IsSameColorBlock(CellData cell, bool isSwap = false)
            {
                if(Block.Attribute.Color == ColorType.None)
                {
                    return false;
                }
                if(cell == null)
                {
                    return false;
                }
                if(!cell.IsVisibleCell)
                {
                    return false;
                }
                if(!cell.Block.HasMiddleBlock)
                {
                    return false;
                }
                if(!cell.Block.MiddleBlock.HasAttribute)
                {
                    return false;
                }
                if (!isSwap && cell.Block.MiddleBlock.IsWillBeUnderMove)
                {
                    return false;
                }
                return Block.Attribute.Color == cell.Block.MiddleBlock.Attribute.Color;
            }
        }
    }
}
