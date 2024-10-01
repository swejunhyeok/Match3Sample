using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {
        public class CellBlock : CellComponent
        {

            #region Unity component

            private Transform _tr;
            public Transform CellTransform
            {
                get
                {
                    if(_tr == null)
                    {
                        _tr = transform;
                    }
                    return _tr;
                }
            }

            #endregion

            #region Block data

            [SerializeField]
            private BlockData _bottomBlock = null;
            [SerializeField]
            private BlockData _middleBlock = null;
            [SerializeField]
            private BlockData _topBlock = null;

            #endregion

            #region Block specific properties

            public bool IsSwapAble
            {
                get
                {
                    if(!Cell.IsVisibleCell)
                    {
                        return false;
                    }
                    if(Cell.State.IsHold)
                    {
                        return false;
                    }
                    if(HasTopBlock)
                    {
                        return false;
                    }
                    if(!HasSwapAbleBlock)
                    {
                        return false;
                    }
                    return true;
                }
            }

            #endregion

            #region Block properties

            public BlockData BottomBlock => _bottomBlock;
            public bool HasBottomBlock => BottomBlock != null;
            public BlockType BottomBlockType => HasBottomBlock && BottomBlock.HasAttribute ? BottomBlock.Attribute.Type : BlockType.None;

            public BlockData MiddleBlock => _middleBlock;
            public bool HasMiddleBlock => MiddleBlock != null;
            public BlockType MiddleBlockType => HasMiddleBlock && MiddleBlock.HasAttribute ? MiddleBlock.Attribute.Type : BlockType.None;

            public BlockData TopBlock => _topBlock;
            public bool HasTopBlock => TopBlock != null;
            public BlockType TopBlockType => HasTopBlock && TopBlock.HasAttribute ? TopBlock.Attribute.Type : BlockType.None;

            public BlockData MoveAbleBlock
            {
                get
                {
                    if (!HasMiddleBlock)
                    {
                        return null;
                    }
                    if (!MiddleBlock.HasAttribute)
                    {
                        return null;
                    }
                    if (!MiddleBlock.Attribute.IsMoveAble)
                    {
                        return null;
                    }
                    return MiddleBlock;
                }
            }
            public BlockData SwapAbleBlock
            {
                get
                {
                    if (!HasMiddleBlock)
                    {
                        return null;
                    }
                    if (!MiddleBlock.HasAttribute)
                    {
                        return null;
                    }
                    if (!MiddleBlock.Attribute.IsSwapAble)
                    {
                        return null;
                    }
                    return MiddleBlock;
                }
            }

            public bool IsEmpty
            {
                get
                {
                    if(!Cell.IsVisibleCell)
                    {
                        return false;
                    }
                    if(_bottomBlock != null)
                    {
                        return false;
                    }
                    if(_middleBlock != null)
                    {
                        return false;
                    }
                    if(_topBlock != null)
                    {
                        return false;
                    }
                    return true;
                }
            }
            public bool HasMoveAbleBlock => MoveAbleBlock != null;
            public bool HasSwapAbleBlock => SwapAbleBlock != null;
            public BlockData HighestBlock
            {
                get
                {
                    if(HasTopBlock)
                    {
                        return TopBlock;
                    }
                    if(HasMiddleBlock)
                    {
                        return MiddleBlock;
                    }
                    if(HasBottomBlock)
                    {
                        return BottomBlock;
                    }
                    return null;
                }
            }

            #endregion

            #region Block manage

            public void CreateBlock(BlockKind kind, BlockType type)
            {
                BlockData block = ObjectPoolManager.Instance.GetObjectData(kind, CellTransform);

                block.SetAttribute(type);

                AddBlock(block);
            }
            
            public void AddBlock(BlockData block, bool isSetPivotCell = true)
            {
                if(block == null)
                {
                    return;
                }
                AddLayerBlock(block);
                block.ChangePivotCell(Cell);
            }

            public void AddLayerBlock(BlockData block)
            {
                if(block == null)
                {
                    return;
                }
                if(!block.HasAttribute)
                {
                    return;
                }
                if(block.Attribute.IsBottomLayer)
                {
                    if(HasBottomBlock)
                    {
                        return;
                    }
                    _bottomBlock = block;
                }
                if(block.Attribute.IsMiddleLayer)
                {
                    if(HasMiddleBlock)
                    {
                        return;
                    }
                    _middleBlock = block;
                }
                if(block.Attribute.IsTopLayer)
                {
                    if(HasTopBlock)
                    {
                        return;
                    }
                    _topBlock = block;
                }
            }

            public void PopBlock(BlockData block)
            {
                if (block == null)
                {
                    return;
                }

                if(BottomBlock == block)
                {
                    _bottomBlock = null;
                }
                if(MiddleBlock == block)
                {
                    _middleBlock = null;
                }
                if(TopBlock == block)
                {
                    _topBlock = null;
                }
            }

            #endregion

            #region Match

            public int MatchPreprocessing(bool isSwap = false)
            {
                int matchCandidateValue = 0;
                if(!HasMiddleBlock)
                {
                    return matchCandidateValue;
                }
                if(!MiddleBlock.HasAttribute)
                {
                    return matchCandidateValue;
                }
                if(MiddleBlock.Attribute.Color == ColorType.None)
                {
                    return matchCandidateValue;
                }
                if(!MiddleBlock.HasMatch)
                {
                    return matchCandidateValue;
                }
                return MiddleBlock.Match.MatchPreprocessing(isSwap);
            }

            public BlockMatch.MatchData MatchCheck(BlockMatch.BlockMatchType type, bool isSwap = false)
            {
                if(!HasMiddleBlock || !MiddleBlock.HasMatch)
                {
                    return new BlockMatch.MatchData() { Type = BlockMatch.BlockMatchType.None };
                }
                return MiddleBlock.Match.MatchSearch(type, isSwap);
            }

            #endregion

        }
    }
}
