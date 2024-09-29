using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {
        public class BlockData : Match3Data
        {
            #region Block attribute

            [SerializeField]
            private BlockAttribute _attribute;
            public BlockAttribute Attribute => _attribute;
            public bool IsContainAttribute => _attribute != null;

            #endregion
        }
    }
}
