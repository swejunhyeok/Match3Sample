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
            private CellState _cState;
            public CellState CState => _cState;

            [SerializeField]
            private CellBlock _cBlock;
            public CellBlock CBlock => _cBlock;

            [SerializeField]
            private CellMove _cMove;
            public CellMove CMove => _cMove;

            [SerializeField]
            private GenerateCell _cGenerate;
            public GenerateCell CGenerate => _cGenerate;

            #endregion

            #region Data load

            public void LoadCellData(LitJson.JsonData cellRoot, Vector2Int pos)
            {
                _pos = pos;

                _cellType = (CellType)InGameUtil.ParseInt(ref cellRoot, ConstantData.MAP_KEY_CELL_TYPE, 1);
                if(IsGenerateCell)
                {
                    CGenerate.LoadGenerateData(cellRoot[ConstantData.MAP_KEY_CELL_GENERATE_LIST]);
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
                        CBlock.CreateBlock(attribute.Kind, type);
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
                    _cState = state;
                }

                CellBlock block = GetComponent<CellBlock>();
                if(block != null)
                {
                    _cBlock = block;
                }

                CellMove move = GetComponent<CellMove>();
                if (move != null)
                {
                    _cMove = move;
                }

                GenerateCell generate = GetComponent<GenerateCell>();
                if(generate != null)
                {
                    _cGenerate = generate;
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
