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

            #endregion

            #region Block component

            [SerializeField]
            private BlockHit _bHit;
            public BlockHit BHit => _bHit;
            public bool HasHit => BHit != null;

            [SerializeField]
            private BlockMatch _bMatch;
            public BlockMatch BMatch => _bMatch;
            public bool HasMatch => BMatch != null;

            [SerializeField]
            private BlockMove _bMove;
            public BlockMove BMove => _bMove;
            public bool HasMove => BMove != null;

            [SerializeField]
            private BlockState _bState;
            public BlockState BState => _bState;
            public bool HasState => BState != null;

            [SerializeField]
            private BlockSprite _bSprite;
            public BlockSprite BSprite => _bSprite;
            public bool HasSprite => BSprite != null;

            #endregion

            #region Cell init

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
                    BSprite.SetSprite(_attribute.SprBlock);
                }
            }

            public void ChangePivotCell(CellData pivotCell, bool isResetPosition = true)
            {
                _pivotCell = pivotCell;
                if(isResetPosition)
                {
                    TrBlock.localPosition = Vector3.zero;
                }
            }

            #endregion

            #region Editor

#if UNITY_EDITOR

            private void Reset()
            {
                BlockHit hit = GetComponent<BlockHit>();
                if(hit != null)
                {
                    _bHit = hit;
                }

                BlockMatch match = GetComponent<BlockMatch>();
                if(match != null)
                {
                    _bMatch = match;
                }

                BlockMove move = GetComponent<BlockMove>();
                if(move != null)
                {
                    _bMove = move;
                }

                BlockState state = GetComponent<BlockState>();
                if(state != null)
                {
                    _bState = state;
                }

                BlockSprite sprite = GetComponent<BlockSprite>();
                if(sprite != null)
                {
                    _bSprite = sprite;
                }
            }

#endif

            #endregion

        }
    }
}
