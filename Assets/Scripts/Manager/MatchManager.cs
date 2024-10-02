using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static JH.Match3Sample.BlockMatch;
using static JH.Match3Sample.BlockMove;

namespace JH
{
    namespace Match3Sample
    {
        public class MatchManager : SingletonManager<MatchManager>
        {

            #region Instance

            void Awake()
            {
                if (_instance == null)
                {
                    _instance = this;
                }

                if (_instance != this)
                {
                    Destroy(gameObject);
                }
            }

            #endregion

            #region Match

            public void MatchCheck()
            {
                List<Vector2Int> rainbowCandidate = new List<Vector2Int>();
                List<Vector2Int> bombCandidate = new List<Vector2Int>();
                List<Vector2Int> rocketCandidate = new List<Vector2Int>();
                List<Vector2Int> propCandidate = new List<Vector2Int>();
                List<Vector2Int> threeCandidate = new List<Vector2Int>();

                for (int y = 0; y < ConstantData.MAX_GRID_HEIGHT_SIZE; ++y)
                {
                    for (int x = 0; x < ConstantData.MAX_GRID_WIDTH_SIZE; ++x)
                    {
                        Vector2Int pos = new Vector2Int(x, y);
                        int candidateValue = GameManager.Instance.GetCell(pos).Block.MatchPreprocessing();
                        if (candidateValue >= 5)
                        {
                            rainbowCandidate.Add(pos);
                        }
                        if (candidateValue >= 4)
                        {
                            bombCandidate.Add(pos);
                        }
                        if (candidateValue >= 3)
                        {
                            rocketCandidate.Add(pos);
                        }
                        //if (candidateValue >= 2)
                        //{
                        //    propCandidate.Add(pos);
                        //}
                        if (candidateValue >= 1)
                        {
                            threeCandidate.Add(pos);
                        }
                        if(candidateValue == 0)
                        {
                            continue;
                        }
                    }
                }

                for (int i = 0; i < rainbowCandidate.Count; ++i)
                {
                    BlockMatch.MatchData matchData = GameManager.Instance.GetCell(rainbowCandidate[i]).Block.MatchCheck(BlockMatch.BlockMatchType.Rainbow);
                    if (matchData.Type != BlockMatch.BlockMatchType.None)
                    {
                        MatchFinish(matchData);
                    }
                }
                for(int i = 0; i < bombCandidate.Count; ++i)
                {
                    BlockMatch.MatchData matchData = GameManager.Instance.GetCell(bombCandidate[i]).Block.MatchCheck(BlockMatch.BlockMatchType.Bomb);
                    if (matchData.Type != BlockMatch.BlockMatchType.None)
                    {
                        MatchFinish(matchData);
                    }
                }
                for (int i = 0; i < rocketCandidate.Count; ++i)
                {
                    BlockMatch.MatchData matchData = GameManager.Instance.GetCell(rocketCandidate[i]).Block.MatchCheck(BlockMatch.BlockMatchType.RocketTransverse);
                    if (matchData.Type != BlockMatch.BlockMatchType.None)
                    {
                        MatchFinish(matchData);
                    }
                }
                for (int i = 0; i < rocketCandidate.Count; ++i)
                {
                    BlockMatch.MatchData matchData = GameManager.Instance.GetCell(rocketCandidate[i]).Block.MatchCheck(BlockMatch.BlockMatchType.RocketVertical);
                    if (matchData.Type != BlockMatch.BlockMatchType.None)
                    {
                        MatchFinish(matchData);
                    }
                }
                //for(int i = 0; i < propCandidate.Count; ++i)
                //{
                //    BlockMatch.MatchData matchData = GameManager.Instance.GetCell(rainbowCandidate[i]).Block.MatchCheck(BlockMatch.BlockMatchType.Prop);
                //    if (matchData.Type != BlockMatch.BlockMatchType.None)
                //    {
                //        MatchFinish(matchData);
                //    }
                //}
                for(int i = 0; i < threeCandidate.Count; ++i)
                {
                    BlockMatch.MatchData matchData = GameManager.Instance.GetCell(threeCandidate[i]).Block.MatchCheck(BlockMatch.BlockMatchType.Three);
                    if (matchData.Type != BlockMatch.BlockMatchType.None)
                    {
                        MatchFinish(matchData);
                    }
                }
            }

            public void SwapMatchCheck(Vector2Int pos)
            {
                CellBlock cellBlock = GameManager.Instance.GetCell(pos).Block;
                int candidateValue = cellBlock.MatchPreprocessing(true);
                BlockMatch.MatchData rainbowMatchData = new BlockMatch.MatchData();
                BlockMatch.MatchData bombMatchData = new BlockMatch.MatchData();
                BlockMatch.MatchData rocketTransverseData = new BlockMatch.MatchData();
                BlockMatch.MatchData rocketVerticalData = new BlockMatch.MatchData();
                BlockMatch.MatchData propData = new BlockMatch.MatchData();
                BlockMatch.MatchData threeData = new BlockMatch.MatchData();

                if(candidateValue >= 5)
                {
                    rainbowMatchData = cellBlock.MatchCheck(BlockMatch.BlockMatchType.Rainbow, true);
                    if(rainbowMatchData.Type != BlockMatch.BlockMatchType.None)
                    {
                        SwapManager.Instance.SetMatchContain(pos);
                    }
                }
                if (candidateValue >= 4)
                {
                    bombMatchData = cellBlock.MatchCheck(BlockMatch.BlockMatchType.Bomb, true);
                    if(bombMatchData.Type != BlockMatch.BlockMatchType.None)
                    {
                        SwapManager.Instance.SetMatchContain(pos);
                    }
                }
                if(candidateValue >= 3)
                {
                    rocketTransverseData = cellBlock.MatchCheck(BlockMatch.BlockMatchType.RocketTransverse, true);
                    if(rocketTransverseData.Type != BlockMatch.BlockMatchType.None)
                    {
                        SwapManager.Instance.SetMatchContain(pos);
                    }
                    rocketVerticalData = cellBlock.MatchCheck(BlockMatch.BlockMatchType.RocketVertical, true);
                    if(rocketVerticalData.Type != BlockMatch.BlockMatchType.None)
                    {
                        SwapManager.Instance.SetMatchContain(pos);
                    }
                }
                //if(candidateValue >= 2)
                //{
                //    propData = cellBlock.MatchCheck(BlockMatch.BlockMatchType.Prop, true);
                //    if(propData.Type != BlockMatch.BlockMatchType.None)
                //    {
                //        SwapManager.Instance.SetMatchContain(pos);
                //    }
                //}
                if(candidateValue >= 1)
                {
                    threeData = cellBlock.MatchCheck(BlockMatch.BlockMatchType.Three, true);
                    if(threeData.Type != BlockMatch.BlockMatchType.None)
                    {
                        SwapManager.Instance.SetMatchContain(pos);
                    }
                }

                SwapManager.Instance.SetMatchCheckCompleted(pos);

                if(rainbowMatchData.Type != BlockMatch.BlockMatchType.None)
                {
                    MatchFinish(rainbowMatchData, true);
                }
                if(bombMatchData.Type != BlockMatch.BlockMatchType.None)
                {
                    MatchFinish(bombMatchData, true);
                }
                if(rocketTransverseData.Type != BlockMatch.BlockMatchType.None)
                {
                    MatchFinish(rocketTransverseData, true);
                }
                if(rocketVerticalData.Type != BlockMatch.BlockMatchType.None)
                {
                    MatchFinish(rocketVerticalData, true);
                }
                //if(propData.Type != BlockMatch.BlockMatchType.None)
                //{
                //    MatchFinish(propData, true);
                //}
                if(threeData.Type != BlockMatch.BlockMatchType.None)
                {
                    MatchFinish(threeData, true);
                }
            }

            public void MatchFinish(BlockMatch.MatchData matchData, bool isSwap = false)
            {
                List<BlockData> hitBlock = new List<BlockData>();
                List<Vector2Int> hitPosition = new List<Vector2Int>();
                switch(matchData.Type)
                {
                    case BlockMatch.BlockMatchType.None:
                    {
                        break;
                    }
                    case BlockMatch.BlockMatchType.Three:
                    {
                        for(int i = 0; i < matchData.MatchInfos.Count; ++i)
                        {
                            CellData cell = matchData.MatchInfos[i].PivotCell;
                            cell.Block.Hit(HitConditionType.Match, LayerType.Middle, BlockType.None, hitBlock, hitPosition);
                        }
                        break;
                    }
                    case BlockMatch.BlockMatchType.RocketTransverse:
                    case BlockMatch.BlockMatchType.RocketVertical:
                    {
                        BlockType makeType = matchData.Type == BlockMatch.BlockMatchType.RocketTransverse ? BlockType.RocketTransverse : BlockType.RocketVertical;
                        Vector2Int pivotPos = GetPivotPos(matchData.MatchInfos);
                        Vector3 pivotTrPosition = GameManager.Instance.GetCell(pivotPos).transform.position;
                        for (int i = 0; i < matchData.MatchInfos.Count; ++i)
                        {
                            CellData cell = matchData.MatchInfos[i].PivotCell;
                            if(cell.Pos == pivotPos)
                            {
                                if(cell.Block.HasMiddleBlock)
                                {
                                    cell.Block.MiddleBlock.Cache.HitBlocks = hitBlock;
                                    cell.Block.MiddleBlock.Cache.HitPositions = hitPosition;
                                }
                                cell.Block.ReservationMakeSpecialBlock = makeType;
                                cell.Block.BlockMove(BlockMove.MoveType.CreateSpecialBlock, pivotTrPosition);
                            }
                            else
                            {
                                if (cell.Block.HasMiddleBlock)
                                {
                                    cell.Block.MiddleBlock.Cache.HitBlocks = hitBlock;
                                    cell.Block.MiddleBlock.Cache.HitPositions = hitPosition;
                                }
                                cell.Block.BlockMove(BlockMove.MoveType.Merge, pivotTrPosition);
                            }
                        }
                        break;
                    }
                    case BlockMatch.BlockMatchType.Prop:
                    {
                        List<CellData> matchCell = new List<CellData>();
                        for (int i = 0; i < matchData.MatchInfos.Count; ++i)
                        {
                            BlockMatch.MatchInfo matchInfo = matchData.MatchInfos[i];
                            CellData pivotCell = matchInfo.PivotCell;

                            matchCell.Add(pivotCell);
                            if(matchInfo.ReverseDirectionIndex == null)
                            {
                                continue;
                            }
                            for(int j = 0; j < matchInfo.ReverseDirectionIndex.Length; ++j)
                            {
                                int directionIndex = matchInfo.ReverseDirectionIndex[j];
                                if(!CellIndex.Verification(pivotCell.Index, CellIndex.FourDirection[directionIndex]))
                                {
                                    continue;
                                }
                                CellData targetCell = pivotCell.FourDirectionCell[directionIndex];
                                if(targetCell == null || !targetCell.IsVisibleCell)
                                {
                                    continue;
                                }
                                if(targetCell.Block.HasMiddleBlock && targetCell.Block.MiddleBlock.State.State != BlockState.BlockStateType.Idle)
                                {
                                    continue;
                                }
                                if(IsSameColorBlock(pivotCell, targetCell))
                                {
                                    matchCell.Add(pivotCell);
                                    targetCell.Block.MiddleBlock.State.SetState(BlockState.BlockStateType.Match);
                                }
                            }
                        }
                        Vector2Int pivotPos = GetPivotPos(matchCell);
                        Vector3 pivotTrPosition = GameManager.Instance.GetCell(pivotPos).transform.position;
                        for (int i = 0; i < matchCell.Count; ++i)
                        {
                            CellData cell = matchCell[i];
                            if (cell.Pos == pivotPos)
                            {
                                cell.Block.ReservationMakeSpecialBlock = BlockType.Prop;
                                if (cell.Block.MiddleBlock != null)
                                {
                                    cell.Block.MiddleBlock.Cache.HitBlocks = hitBlock;
                                    cell.Block.MiddleBlock.Cache.HitPositions = hitPosition;
                                }
                                cell.Block.BlockMove(BlockMove.MoveType.CreateSpecialBlock, pivotTrPosition);
                            }
                            else
                            {
                                if (cell.Block.MiddleBlock != null)
                                {
                                    cell.Block.MiddleBlock.Cache.HitBlocks = hitBlock;
                                    cell.Block.MiddleBlock.Cache.HitPositions = hitPosition;
                                }
                                cell.Block.BlockMove(BlockMove.MoveType.Merge, pivotTrPosition);
                            }
                        }
                        break;
                    }
                    case BlockMatch.BlockMatchType.Bomb:
                    case BlockMatch.BlockMatchType.Rainbow:
                    {
                        List<CellData> matchCell = new List<CellData>();
                        for (int i = 0; i < matchData.MatchInfos.Count; ++i)
                        {
                            BlockMatch.MatchInfo macthInfo = matchData.MatchInfos[i];
                            CellData pivotCell = macthInfo.PivotCell;
                            matchCell.Add(pivotCell);

                            int sameDirectionIndex = macthInfo.SameDirectionIndex;
                            CellData targetCell = null;
                            if (sameDirectionIndex != -1 && CellIndex.Verification(pivotCell.Index, CellIndex.FourDirection[sameDirectionIndex]))
                            {
                                targetCell = pivotCell.FourDirectionCell[sameDirectionIndex];
                                for (int k = 0; k < 2; ++k)
                                {
                                    if (targetCell == null || !targetCell.IsVisibleCell)
                                    {
                                        break;
                                    }
                                    if (targetCell.Block.HasMiddleBlock && targetCell.Block.MiddleBlock.State.State != BlockState.BlockStateType.Idle)
                                    {
                                        continue;
                                    }
                                    if (IsSameColorBlock(pivotCell, targetCell))
                                    {
                                        matchCell.Add(targetCell);
                                        targetCell.Block.MiddleBlock.State.SetState(BlockState.BlockStateType.Match);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                    if (!CellIndex.Verification(targetCell.Index, CellIndex.FourDirection[sameDirectionIndex]))
                                    {
                                        break;
                                    }
                                    targetCell = targetCell.FourDirectionCell[sameDirectionIndex];
                                }
                            }

                            List<CellData> reverseSameCell = new List<CellData>();
                            if (macthInfo.ReverseDirectionIndex != null)
                            {
                                for (int j = 0; j < macthInfo.ReverseDirectionIndex.Length; ++j)
                                {
                                    int reverseDirectionIndex = macthInfo.ReverseDirectionIndex[j];
                                    if (!CellIndex.Verification(pivotCell.Index, CellIndex.FourDirection[reverseDirectionIndex]))
                                    {
                                        continue;
                                    }
                                    targetCell = pivotCell.FourDirectionCell[reverseDirectionIndex];
                                    for (int k = 0; k < 2; ++k)
                                    {
                                        if (targetCell == null || !targetCell.IsVisibleCell)
                                        {
                                            break;
                                        }
                                        if (targetCell.Block.HasMiddleBlock && targetCell.Block.MiddleBlock.State.State != BlockState.BlockStateType.Idle)
                                        {
                                            continue;
                                        }
                                        if (IsSameColorBlock(pivotCell, targetCell))
                                        {
                                            matchCell.Add(targetCell);
                                            targetCell.Block.MiddleBlock.State.SetState(BlockState.BlockStateType.Match);
                                        }
                                        else
                                        {
                                            break;
                                        }
                                        if (!CellIndex.Verification(targetCell.Index, CellIndex.FourDirection[reverseDirectionIndex]))
                                        {
                                            break;
                                        }
                                        targetCell = targetCell.FourDirectionCell[reverseDirectionIndex];
                                    }
                                }
                            }

                            if (reverseSameCell.Count >= 2)
                            {
                                for (int j = 0; j < reverseSameCell.Count; ++j)
                                {
                                    reverseSameCell[j].Block.MiddleBlock.State.SetState(BlockState.BlockStateType.Match);
                                    matchCell.Add(reverseSameCell[j]);
                                }
                            }
                        }
                        BlockType type = matchData.Type == BlockMatchType.Bomb ? BlockType.Bomb : BlockType.Rainbow;

                        Vector2Int pivotPos = GetPivotPos(matchCell);
                        Vector3 pivotTrPosition = GameManager.Instance.GetCell(pivotPos).transform.position;
                        for (int i = 0; i < matchCell.Count; ++i)
                        {
                            CellData cell = matchCell[i];
                            if (cell.Pos == pivotPos)
                            {
                                cell.Block.ReservationMakeSpecialBlock = type;
                                if (cell.Block.MiddleBlock != null)
                                {
                                    cell.Block.MiddleBlock.Cache.HitBlocks = hitBlock;
                                    cell.Block.MiddleBlock.Cache.HitPositions = hitPosition;
                                }
                                cell.Block.BlockMove(BlockMove.MoveType.CreateSpecialBlock, pivotTrPosition);
                            }
                            else
                            {
                                if (cell.Block.MiddleBlock != null)
                                {
                                    cell.Block.MiddleBlock.Cache.HitBlocks = hitBlock;
                                    cell.Block.MiddleBlock.Cache.HitPositions = hitPosition;
                                }
                                cell.Block.BlockMove(BlockMove.MoveType.Merge, pivotTrPosition);
                            }
                        }
                        break;
                    }
                }

            }

            private bool IsSameColorBlock(CellData srcCell1, CellData srcCell2)
            {
                if(!srcCell1.Block.HasMiddleBlock || !srcCell1.Block.MiddleBlock.HasAttribute)
                {
                    return false;
                }
                if(!srcCell2.Block.HasMiddleBlock || !srcCell2.Block.MiddleBlock.HasAttribute)
                {
                    return false;
                }
                return srcCell1.Block.MiddleBlock.Attribute.Color == srcCell2.Block.MiddleBlock.Attribute.Color;
            }

            private Vector2Int GetPivotPos(List<BlockMatch.MatchInfo> matchElement)
            {
                Vector2Int pivotPos = CellIndex.None;
                GameTimeData pivotGameTimeData = new GameTimeData();
                for (int i = 0; i < matchElement.Count; ++i)
                {
                    CellData cellData = matchElement[i].PivotCell;
                    if (cellData == null)
                    {
                        continue;
                    }
                    if (!cellData.Block.HasMiddleBlock)
                    {
                        continue;
                    }
                    GameTimeData gameTimeData = cellData.Block.MiddleBlock.Move.MoveStopTimeData;
                    if (pivotGameTimeData.Version < gameTimeData.Version)
                    {
                        pivotGameTimeData = gameTimeData;
                        pivotPos = cellData.Pos;
                    }
                    else if (pivotGameTimeData.Version == gameTimeData.Version
                        && pivotGameTimeData.DeltaTime < gameTimeData.DeltaTime)
                    {
                        pivotGameTimeData = gameTimeData;
                        pivotPos = cellData.Pos;
                    }
                }
                return pivotPos;
            }

            private Vector2Int GetPivotPos(List<CellData> cellDatas)
            {
                Vector2Int pivotPos = CellIndex.None;
                if (cellDatas.Count == 0)
                {
                    return pivotPos;
                }
                else
                {
                    for (int i = 0; i < cellDatas.Count; ++i)
                    {
                        CellData cellData = cellDatas[i];
                        if (cellData == null)
                        {
                            continue;
                        }
                        if (!cellData.Block.HasMiddleBlock)
                        {
                            continue;
                        }
                        pivotPos = cellData.Pos;
                        break;
                    }
                }
                GameTimeData pivotGameTimeData = new GameTimeData();
                for (int i = 0; i < cellDatas.Count; ++i)
                {
                    CellData cellData = cellDatas[i];
                    if (cellData == null)
                    {
                        continue;
                    }
                    if (!cellData.Block.HasMiddleBlock)
                    {
                        continue;
                    }
                    GameTimeData gameTimeData = cellData.Block.MiddleBlock.Move.MoveStopTimeData;
                    if (pivotGameTimeData.Version < gameTimeData.Version)
                    {
                        pivotGameTimeData = gameTimeData;
                        pivotPos = cellData.Pos;
                    }
                    else if (pivotGameTimeData.Version == gameTimeData.Version
                        && pivotGameTimeData.DeltaTime < gameTimeData.DeltaTime)
                    {
                        pivotGameTimeData = gameTimeData;
                        pivotPos = cellData.Pos;
                    }
                }
                return pivotPos;
            }

            private void Update()
            {
                if (GameManager.Instance.IsContainState(GameManager.GameState.GameStart))
                {
                    return;
                }
                if (GameManager.Instance.IsContainState(GameManager.GameState.Loading_GridData))
                {
                    return;
                }
                MatchCheck();
            }

            #endregion


        }
    }
}
