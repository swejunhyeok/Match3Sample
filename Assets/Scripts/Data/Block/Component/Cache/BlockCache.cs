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

            #region General

            public override void Initialize()
            {
                base.Initialize();
                MoveEndAction = null;
            }

            public override void Dispose()
            {
                base.Dispose();
                MoveEndAction = null;
            }

            #endregion

        }
    }
}
