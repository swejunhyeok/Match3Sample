using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {
        public class BlockState : BlockComponent
        {
            public enum BlockStateType
            {
                Idle,
                Move,
                Match,
                Destroyed,
            }

            [SerializeField]
            private BlockStateType _state;
            public BlockStateType State => _state;

            public void SetState(BlockStateType state)
            {
                _state = state;
            }

            #region General

            public override void Initialize()
            {
                base.Initialize();
                _state = BlockStateType.Idle;
            }

            public override void Dispose()
            {
                base.Dispose();
                _state = BlockStateType.Destroyed;
            }

            #endregion

        }
    }
}
