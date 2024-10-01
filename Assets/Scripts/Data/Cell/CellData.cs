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
            public bool IsGenerateCell => _cellType == CellType.GenerateCell;
            public bool IsWorkCell => _cellType == CellType.NormalCell || _cellType == CellType.FinishCell || _cellType == CellType.GenerateCell;

            [SerializeField]
            private Vector2Int _pos;
            public Vector2Int Pos => _pos;

            [SerializeField]
            private int _index;
            public int Index => _index;

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

            [SerializeField]
            private GenerateCell _generate;
            public GenerateCell Generate => _generate;

            #endregion

            #region Data load

            public void LoadCellData(LitJson.JsonData cellRoot, Vector2Int pos, int index)
            {
                _pos = pos;
                _index = index;

                _cellType = (CellType)InGameUtil.ParseInt(ref cellRoot, ConstantData.MAP_KEY_CELL_TYPE, 1);
                if(IsGenerateCell)
                {
                    Generate.LoadGenerateData(cellRoot[ConstantData.MAP_KEY_CELL_GENERATE_LIST]);
                }
            }

            public void LoadBlockData(LitJson.JsonData cellRoot)
            {
                if(IsEmptyCell)
                {
                    return;
                }

                if(!cellRoot.Keys.Contains(ConstantData.MAP_KEY_BLOCK_LIST))
                {
                    return;
                }

                LitJson.JsonData blocksData = cellRoot[ConstantData.MAP_KEY_BLOCK_LIST];

                for(int i = 0; i < blocksData.Count; ++i)
                {
                    LitJson.JsonData blockData = blocksData[i];
                    BlockType type = (BlockType)InGameUtil.ParseInt(ref blockData, ConstantData.MAP_KEY_BLOCK_TYPE, 0);
                    BlockAttribute attribute = AddressableManager.Instance.GetBlockAttribute(type);
                    if (attribute != null)
                    {
                        Block.CreateBlock(attribute.Kind, type);
                    }
                }
            }

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

                GenerateCell generate = GetComponent<GenerateCell>();
                if(generate != null)
                {
                    _generate = generate;
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
