using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {
        public class BlockData : Match3Data
        {

            #region Unity component

            private Transform _trBlock;
            public Transform TrBlock
            {
                get
                {
                    if(_trBlock == null)
                    {
                        _trBlock = transform;
                    }
                    return _trBlock;
                }
            }

            #endregion

            #region Block attribute

            [SerializeField]
            private BlockAttribute _attribute;
            public BlockAttribute Attribute => _attribute;
            public bool HasAttribute => _attribute != null;

            #endregion

            #region Block data

            [SerializeField]
            private CellData _pivotCell;
            public CellData PivotCell => _pivotCell;

            #endregion

            #region Block component

            [SerializeField]
            private BlockCache _cache;
            public BlockCache Cache => _cache;
            public bool HasCache => _cache != null;

            [SerializeField]
            private BlockHit _hit;
            public BlockHit Hit => _hit;
            public bool HasHit => Hit != null;

            [SerializeField]
            private BlockMatch _match;
            public BlockMatch Match => _match;
            public bool HasMatch => Match != null;

            [SerializeField]
            private BlockMove _move;
            public BlockMove Move => _move;
            public bool HasMove => Move != null;

            [SerializeField]
            private BlockState _state;
            public BlockState State => _state;
            public bool HasState => State != null;

            [SerializeField]
            private BlockSprite _sprite;
            public BlockSprite Sprite => _sprite;
            public bool HasSprite => Sprite != null;

            #endregion

            #region Block state

            public bool IsWillBeUnderMove
            {
                get
                {
                    if(PivotCell == null)
                    {
                        return false;
                    }
                    if(Attribute == null)
                    {
                        return false;
                    }
                    if(!Attribute.IsMoveAble)
                    {
                        return false;
                    }
                    if(PivotCell.DownCell == null)
                    {
                        return false;
                    }
                    if(PivotCell.DownCell.Block.HasMiddleBlock)
                    {
                        return false;
                    }
                    return true;
                }
            }

            #endregion

            #region General

            public override void Init()
            {
                base.Init();
                _pivotCell = null;
                if(HasCache)
                {
                    Cache.Initialize();
                }
                if(HasHit)
                {
                    Hit.Initialize();
                }
                if(HasMatch)
                {
                    Match.Initialize();
                }
                if(HasMove)
                {
                    Move.Initialize();
                }
                if(HasSprite)
                {
                    Sprite.Initialize();
                }
                if(HasState)
                {
                    State.Initialize();
                }
            }

            public override void Dispose(bool isForce = false)
            {
                base.Dispose(isForce);
                if (HasCache)
                {
                    Cache.Dispose();
                }
                if (HasHit)
                {
                    Hit.Dispose();
                }
                if (HasMatch)
                {
                    Match.Dispose();
                }
                if (HasMove)
                {
                    Move.Dispose();
                }
                if (HasSprite)
                {
                    Sprite.Dispose();
                }
                if (HasState)
                {
                    State.Dispose();
                }

                _pivotCell = null;
                ObjectPoolManager.Instance.Dispose(this);
            }

            #endregion

            #region Data control

            public void SetAttribute(BlockType type)
            {
                _attribute = AddressableManager.Instance.GetBlockAttribute(type);
                OnChangeAttribute();
            }

            public void ChangeAttribute(BlockAttribute attribute)
            {
                _attribute = attribute;
                OnChangeAttribute();
            }

            private void OnChangeAttribute()
            {
                if(HasSprite)
                {
                    Sprite.SetSprite(_attribute.SprBlock);
                }
            }

            public void ChangePivotCell(CellData pivotCell, bool isResetPosition = true)
            {
                _pivotCell = pivotCell;
                transform.parent = _pivotCell.transform;
                if(isResetPosition)
                {
                    TrBlock.localPosition = Vector3.zero;
                }
            }

            public void RemoveCell()
            {
                _pivotCell.Block.PopBlock(this);
                _pivotCell = null;
            }

            #endregion

            #region Editor

#if UNITY_EDITOR

            private void Reset()
            {
                BlockCache cache = GetComponent<BlockCache>();
                if(cache != null)
                {
                    _cache = cache;
                }

                BlockHit hit = GetComponent<BlockHit>();
                if(hit != null)
                {
                    _hit = hit;
                }

                BlockMatch match = GetComponent<BlockMatch>();
                if(match != null)
                {
                    _match = match;
                }

                BlockMove move = GetComponent<BlockMove>();
                if(move != null)
                {
                    _move = move;
                }

                BlockState state = GetComponent<BlockState>();
                if(state != null)
                {
                    _state = state;
                }

                BlockSprite sprite = GetComponent<BlockSprite>();
                if(sprite != null)
                {
                    _sprite = sprite;
                }
            }

#endif

            #endregion

        }
    }
}
