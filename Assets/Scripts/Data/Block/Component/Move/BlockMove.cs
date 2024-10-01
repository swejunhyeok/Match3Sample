using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {
        public class BlockMove : BlockComponent
        {

            public enum MoveType
            {
                Swap,
            }

            public void Move()
            {
                
            }

            #region Move start func

            public void CommonMoveStart()
            {
                Block.State.SetState(BlockState.BlockStateType.Move);
            }

            #endregion

            #region Move end func

            public void CommonMoveEnd()
            {
                Block.State.SetState(BlockState.BlockStateType.Idle);
                Block.Cache.MoveEndAction?.Invoke();
            }

            #endregion

        }
    }
}
