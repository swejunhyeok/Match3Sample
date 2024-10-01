using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                        if (candidateValue >= 2)
                        {
                            propCandidate.Add(pos);
                        }
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
                        MatchComplete(matchData);
                    }
                }
                for(int i = 0; i < bombCandidate.Count; ++i)
                {
                    BlockMatch.MatchData matchData = GameManager.Instance.GetCell(rainbowCandidate[i]).Block.MatchCheck(BlockMatch.BlockMatchType.Bomb);
                    if (matchData.Type != BlockMatch.BlockMatchType.None)
                    {
                        MatchComplete(matchData);
                    }
                }
                for (int i = 0; i < rocketCandidate.Count; ++i)
                {
                    BlockMatch.MatchData matchData = GameManager.Instance.GetCell(rainbowCandidate[i]).Block.MatchCheck(BlockMatch.BlockMatchType.RocketTransverse);
                    if (matchData.Type != BlockMatch.BlockMatchType.None)
                    {
                        MatchComplete(matchData);
                    }
                }
                for (int i = 0; i < rocketCandidate.Count; ++i)
                {
                    BlockMatch.MatchData matchData = GameManager.Instance.GetCell(rainbowCandidate[i]).Block.MatchCheck(BlockMatch.BlockMatchType.RocketVertical);
                    if (matchData.Type != BlockMatch.BlockMatchType.None)
                    {
                        MatchComplete(matchData);
                    }
                }
                for(int i = 0; i < propCandidate.Count; ++i)
                {
                    BlockMatch.MatchData matchData = GameManager.Instance.GetCell(rainbowCandidate[i]).Block.MatchCheck(BlockMatch.BlockMatchType.Prop);
                    if (matchData.Type != BlockMatch.BlockMatchType.None)
                    {
                        MatchComplete(matchData);
                    }
                }
                for(int i = 0; i < threeCandidate.Count; ++i)
                {
                    BlockMatch.MatchData matchData = GameManager.Instance.GetCell(rainbowCandidate[i]).Block.MatchCheck(BlockMatch.BlockMatchType.Three);
                    if (matchData.Type != BlockMatch.BlockMatchType.None)
                    {
                        MatchComplete(matchData);
                    }
                }
            }

            public void MatchComplete(BlockMatch.MatchData matchData)
            {

            }

            #endregion


        }
    }
}
