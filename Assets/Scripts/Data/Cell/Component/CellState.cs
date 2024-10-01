using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {
        public class CellState : CellComponent
        {

            #region Hold

            [SerializeField]
            private int _cntHold = 0;
            
            public bool IsHold => _cntHold > 0;

            public void AddHoldState() { ++_cntHold; }
            public void ReduceHoldState() { --_cntHold; }

            #endregion

        }
    }
}
