using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {
        public class CellData : Match3Data
        {
            #region Cell data

            public enum CellType
            {
                None,
                EmptyCell,
                NormalCell,
                GenerateCell,
                FinishCell,
            }

            [SerializeField]
            private CellType _cellType;
            
            public bool IsEmptyCell => _cellType == CellType.EmptyCell;
            public bool IsVisibleCell => _cellType == CellType.NormalCell || _cellType == CellType.FinishCell;

            private CellData[] _fourDirectionCell = new CellData[4];
            public CellData[] FourDirectionCell => _fourDirectionCell;
            public CellData UpCell => _fourDirectionCell[CellIndex.UpIndex];
            public CellData DownCell => _fourDirectionCell[CellIndex.DownIndex];
            public CellData LeftCell => _fourDirectionCell[CellIndex.LeftIndex];
            public CellData RightCell => _fourDirectionCell[CellIndex.RightIndex];

            private CellData[] _fourDiagonalCell = new CellData[4];
            public CellData[] FourDiagonalCell => _fourDiagonalCell;
            public CellData UpLeftCell => _fourDiagonalCell[CellIndex.UpLeftIndex];
            public CellData UpRightCell => _fourDiagonalCell[CellIndex.UpRightIndex];
            public CellData DownLeftCell => _fourDiagonalCell[CellIndex.DownLeftIndex];
            public CellData DownRightCell => _fourDiagonalCell[CellIndex.DownRightIndex];

            public void SetArroundCell(CellData[] fourDirectionCell, CellData[] fourDiagonalCell)
            {
                _fourDirectionCell = fourDirectionCell;
                _fourDiagonalCell = fourDiagonalCell;
            }

            #endregion

            #region Cell component

            [SerializeField]
            private CellState _state;
            public CellState State => _state;

            [SerializeField]
            private CellBlock _block;
            public CellBlock Block => _block;

            [SerializeField]
            private CellMove _move;
            public CellMove Move => _move;

            #endregion

            #region Editor

#if UNITY_EDITOR

            private void RegistCellComponent()
            {
                CellState state = GetComponent<CellState>();
                if(state != null)
                {
                    _state = state;
                }

                CellBlock block = GetComponent<CellBlock>();
                if(block != null)
                {
                    _block = block;
                }

                CellMove move = GetComponent<CellMove>();
                if (move != null)
                {
                    _move = move;
                }
            }

            private void Reset()
            {
                RegistCellComponent();
            }

#endif

            #endregion

        }
    }
}
