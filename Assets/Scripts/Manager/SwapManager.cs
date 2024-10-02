using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {
        public class SwapManager : SingletonManager<SwapManager>
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

            #region Swap

            [System.Serializable]
            public class SwapData
            {
                [System.Serializable]
                public class EachSwapData
                {
                    public Vector2Int Position;
                    public bool IsMoveCompleted = false;
                    public bool IsContainMatch = false;
                    public bool IsMatchCheckCompleted = false;
                }
                public EachSwapData Pivot = new EachSwapData();
                public EachSwapData Target = new EachSwapData();
            }

            [SerializeField]
            private List<SwapData> _swapDatas = new List<SwapData>();

            public void AddSwapInfo(Vector2Int pivotPos, Vector2Int targetPos, bool isPivotGimmick = false, bool isTargetGimmick = false)
            {
                SwapData swapData = new SwapData();
                swapData.Pivot.Position = pivotPos;
                swapData.Pivot.IsMoveCompleted = pivotPos == CellIndex.None;
                swapData.Pivot.IsMatchCheckCompleted = isPivotGimmick ? true : pivotPos == CellIndex.None;
                swapData.Target.Position = targetPos;
                swapData.Target.IsMoveCompleted = targetPos == CellIndex.None;
                swapData.Target.IsMatchCheckCompleted = isTargetGimmick ? true : targetPos == CellIndex.None;
                _swapDatas.Add(swapData);
            }

            private void CheckSwapData()
            {
                for(int i = _swapDatas.Count - 1; i >= 0; --i)
                {
                    if (!_swapDatas[i].Pivot.IsMoveCompleted)
                    {
                        continue;
                    }
                    if (!_swapDatas[i].Pivot.IsMatchCheckCompleted)
                    {
                        continue;
                    }
                    if (!_swapDatas[i].Target.IsMoveCompleted)
                    {
                        continue;
                    }
                    if (!_swapDatas[i].Target.IsMatchCheckCompleted)
                    {
                        continue;
                    }
                    if (_swapDatas[i].Pivot.IsContainMatch || _swapDatas[i].Target.IsContainMatch)
                    {
                        --GameManager.Instance.Move;
                    }
                    else
                    {
                        GameManager.Instance.Grid.ReverseSwap(_swapDatas[i].Pivot.Position, _swapDatas[i].Target.Position);
                    }
                    GameManager.Instance.RemoveGameState(GameManager.GameState.Processing_UserInput);
                    _swapDatas.RemoveAt(i);
                }
            }

            public void SetMatchCheckCompleted(Vector2Int pos)
            {
                for(int i = 0; i < _swapDatas.Count; ++i)
                {
                    if (_swapDatas[i].Pivot.Position == pos)
                    {
                        _swapDatas[i].Pivot.IsMatchCheckCompleted = true;
                    }
                    if (_swapDatas[i].Target.Position == pos)
                    {
                        _swapDatas[i].Target.IsMatchCheckCompleted = true;
                    }
                }
                CheckSwapData();
            }

            public void SetMatchContain(Vector2Int pos)
            {
                for (int i = 0; i < _swapDatas.Count; ++i)
                {
                    if (_swapDatas[i].Pivot.Position == pos)
                    {
                        _swapDatas[i].Pivot.IsContainMatch = true;
                    }
                    if (_swapDatas[i].Target.Position == pos)
                    {
                        _swapDatas[i].Target.IsContainMatch = true;
                    }
                }
                CheckSwapData();
            }

            public void SetMoveCompleted(Vector2Int pos)
            {
                for(int i = 0; i < _swapDatas.Count; ++i)
                {
                    if (_swapDatas[i].Pivot.Position == pos)
                    {
                        _swapDatas[i].Pivot.IsMoveCompleted = true;
                    }
                    if (_swapDatas[i].Target.Position == pos)
                    {
                        _swapDatas[i].Target.IsMoveCompleted = true;
                    }
                }
                CheckSwapData();
                MatchManager.Instance.SwapMatchCheck(pos);
            }

            public bool IsContainSwapData(Vector2Int pos)
            {
                for(int i = 0; i < _swapDatas.Count; ++i)
                {
                    if (_swapDatas[i].Pivot.Position == pos)
                    {
                        return true;
                    }
                    if (_swapDatas[i].Target.Position == pos)
                    {
                        return true;
                    }
                }
                return false;
            }

            #endregion

        }
    }
}
