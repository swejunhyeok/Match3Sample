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
                Destroied,
            }

            [SerializeField]
            private BlockStateType _state;
            public BlockStateType State => _state;

            public void SetState(BlockStateType state)
            {
                _state = state;
            }
            
        }
    }
}
