using System.Collections;
using System.Collections.Generic;
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

            #region Block properties

            public BlockData BottomBlock => _bottomBlock;
            public bool IsContainBottomBlock => BottomBlock != null;
            public BlockType BottomBlockType => IsContainBottomBlock && BottomBlock.IsContainAttribute ? BottomBlock.Attribute.Type : BlockType.None;

            public BlockData MiddleBlock => _middleBlock;
            public bool IsContainMiddleBlock => MiddleBlock != null;
            public BlockType MiddleBlockType => IsContainMiddleBlock && MiddleBlock.IsContainAttribute ? MiddleBlock.Attribute.Type : BlockType.None;

            public BlockData TopBlock => _topBlock;
            public bool IsContainTopBlock => TopBlock != null;
            public BlockType TopBlockType => IsContainTopBlock && TopBlock.IsContainAttribute ? TopBlock.Attribute.Type : BlockType.None;

            public BlockData HighestBlock
            {
                get
                {
                    if(IsContainTopBlock)
                    {
                        return TopBlock;
                    }
                    if(IsContainMiddleBlock)
                    {
                        return MiddleBlock;
                    }
                    if(IsContainBottomBlock)
                    {
                        return BottomBlock;
                    }
                    return null;
                }
            }

            #endregion

            #region Block manage

            public BlockData CreateBlock(BlockKind kind, BlockType type)
            {
                BlockData block = ObjectPoolManager.Instance.GetObjectData(kind, CellTransform);

                return block;
            }

            public void AddLayerBlock(BlockData block)
            {
                if(block == null)
                {
                    return;
                }
                if(!block.IsContainAttribute)
                {
                    return;
                }
                if(block.Attribute.IsBottomLayer)
                {
                    if(IsContainBottomBlock)
                    {
                        return;
                    }
                    _bottomBlock = block;
                }
                if(block.Attribute.IsMiddleLayer)
                {
                    if(IsContainMiddleBlock)
                    {
                        return;
                    }
                    _middleBlock = block;
                }
                if(block.Attribute.IsTopLayer)
                {
                    if(IsContainTopBlock)
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

        }
    }
}
