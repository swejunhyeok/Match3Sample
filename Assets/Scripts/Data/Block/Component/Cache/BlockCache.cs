using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {
        public class BlockCache : BlockComponent
        {

            #region Move component caching data

            public System.Action MoveEndAction;

            #endregion

            #region Hit component caching data

            public List<BlockData> HitBlocks = null;
            public List<Vector2Int> HitPositions = null;

            #endregion

            #region General

            public override void Initialize()
            {
                base.Initialize();
                MoveEndAction = null;
                HitBlocks = null;
                HitPositions = null;
            }

            public override void Dispose()
            {
                base.Dispose();
                MoveEndAction = null;
                HitBlocks = null;
                HitPositions = null;
            }

            #endregion

        }
    }
}
