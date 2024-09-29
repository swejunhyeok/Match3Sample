using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {
        public class BlockComponent : MonoBehaviour
        {

            #region Block data

            [SerializeField]
            protected BlockData _block;
            protected BlockData Block => _block;

            #endregion

            #region Editor

#if UNITY_EDITOR

            protected void RegistBlock()
            {
                BlockData block = GetComponent<BlockData>();
                if(block != null)
                {
                    _block = block;
                }
            }

            protected virtual void Reset()
            {
                RegistBlock();
            }
#endif

            #endregion

        }
    }
}
