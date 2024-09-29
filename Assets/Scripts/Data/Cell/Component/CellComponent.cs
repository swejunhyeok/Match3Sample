using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {
        public class CellComponent : MonoBehaviour
        {

            #region Cell data

            [SerializeField]
            protected CellData _cell;

            protected CellData Cell
            {
                get => _cell;
            }

            #endregion

            #region Editor

#if UNITY_EDITOR

            private void RegistCell()
            {
                CellData cell = GetComponent<CellData>();
                if(cell == null)
                {
                    return;
                }
                _cell = cell;
            }

            private void Reset()
            {
                RegistCell();
            }

#endif

            #endregion

        }
    }
}
