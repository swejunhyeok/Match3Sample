using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

namespace JH
{
    namespace Match3Sample
    {
        public class BlockMove : BlockComponent
        {

            public GameTimeData MoveStopTimeData = new GameTimeData();

            public enum MoveType
            {
                Swap,
                CreateSpecialBlock,
                Merge,
                ReverseSwap,
                BlockDown,
            }

            public void Move(MoveType type)
            {
                OnMoveStart(type);
                StartCoroutine(MoveLocalPositionZero(type));
            }
            public void Move(MoveType type, Vector3 targetPosition)
            {
                OnMoveStart(type);
                StartCoroutine(MoveSpecificPosition(type, targetPosition));
            }

            protected IEnumerator MoveLocalPositionZero(MoveType type, float duration = 0.25f)
            {
                float timeDepth = InGameUtil.GetTimeDepth(duration);
                float deltaSum = 0f;
                Vector2 orgPosition = transform.localPosition;
                while(deltaSum < 1f)
                {
                    deltaSum += timeDepth * Time.deltaTime;
                    transform.localPosition = Vector2.Lerp(orgPosition, Vector2.zero, deltaSum);
                    if(deltaSum >= 1f)
                    {
                        break;
                    }
                    yield return null;
                }
                transform.localPosition = Vector3.zero;
                OnMoveEnd(type);
            }

            protected IEnumerator MoveSpecificPosition(MoveType type, Vector3 targetPosition, float duration = 0.25f)
            {
                float timeDepth = InGameUtil.GetTimeDepth(duration);
                float deltaSum = 0f;
                Vector3 orgPosition = transform.position;
                while (deltaSum < 1f)
                {
                    deltaSum += timeDepth * Time.deltaTime;
                    transform.position = Vector3.Lerp(orgPosition, targetPosition, deltaSum);
                    if (deltaSum >= 1f)
                    {
                        break;
                    }
                    yield return null;
                }
                transform.position = targetPosition;
                OnMoveEnd(type);
            }

            #region Move start func

            protected void OnMoveStart(MoveType type)
            {
                CommonMoveStart();
                switch (type)
                {
                    case MoveType.Swap:
                    {
                        OnSwapStart();
                        break;
                    }
                    case MoveType.CreateSpecialBlock:
                    {
                        OnCreateSpecialBlockStart();
                        break;
                    }
                    case MoveType.Merge:
                    {
                        OnMergeStart();
                        break;
                    }
                    case MoveType.ReverseSwap:
                    {
                        OnReverseSwapStart();
                        break;
                    }
                    case MoveType.BlockDown:
                    {
                        OnBlockDownStart();
                        break;
                    }
                }
            }

            protected void CommonMoveStart()
            {
                Block.State.SetState(BlockState.BlockStateType.Move);
            }

            protected virtual void OnSwapStart() { }
            protected virtual void OnCreateSpecialBlockStart() { }
            protected virtual void OnMergeStart() { }
            protected virtual void OnReverseSwapStart() { }
            protected virtual void OnBlockDownStart() { }

            #endregion

            #region Move end func

            protected void OnMoveEnd(MoveType type)
            {
                CommonMoveEnd();
                switch(type)
                {
                    case MoveType.Swap:
                    {
                        OnSwapEnd();
                        break;
                    }
                    case MoveType.CreateSpecialBlock:
                    {
                        OnCreateSpecialBlockEnd();
                        break;
                    }
                    case MoveType.Merge:
                    {
                        OnMergeEnd();
                        break;
                    }
                    case MoveType.ReverseSwap:
                    {
                        OnReverseSwapEnd();
                        break;
                    }
                    case MoveType.BlockDown:
                    {
                        OnBlockDownEnd();
                        break;
                    }
                }
            }

            protected void CommonMoveEnd()
            {
                if (Block.State.State != BlockState.BlockStateType.Destroyed)
                {
                    Block.State.SetState(BlockState.BlockStateType.Idle);
                }
                Block.Cache.MoveEndAction?.Invoke();
                Block.Cache.MoveEndAction = null;
                MoveStopTimeData = GameManager.Instance.GameTime;
            }

            protected virtual void OnSwapEnd() 
            {
                SwapManager.Instance.SetMoveCompleted(Block.PivotCell.Pos);
            }
            protected virtual void OnCreateSpecialBlockEnd() 
            {
                CellData pivotCell = Block.PivotCell;
                pivotCell.Block.Hit(HitConditionType.Merge, LayerType.Middle, BlockType.None, Block.Cache.HitBlocks, Block.Cache.HitPositions,
                    successCallback: () =>
                    {
                        if (pivotCell.Block.ReservationMakeSpecialBlock != BlockType.None)
                        {
                            pivotCell.Block.CreateBlock(BlockKind.SpecialBlock, pivotCell.Block.ReservationMakeSpecialBlock);
                        }
                    });
            }
            protected virtual void OnMergeEnd()
            {
                CellData pivotCell = Block.PivotCell;
                pivotCell.Block.Hit(HitConditionType.Merge, LayerType.Middle, BlockType.None, Block.Cache.HitBlocks, Block.Cache.HitPositions);
            }
            protected virtual void OnReverseSwapEnd() { }
            protected virtual void OnBlockDownEnd() { }

            #endregion

            #region General

            public override void Initialize()
            {
                base.Initialize();
                MoveStopTimeData = new GameTimeData();
            }

            public override void Dispose()
            {
                base.Dispose();
                MoveStopTimeData = new GameTimeData();
            }

            #endregion

        }
    }
}
