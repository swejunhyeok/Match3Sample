using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace JH
{
    namespace Match3Sample
    {
        public class GameManager : SingletonManager<GameManager>
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

            #region Unity component

            [SerializeField]
            private Transform _trLevel;

            #endregion

            #region Game data

            [SerializeField]
            private int _levelNum = 0;

            [SerializeField]
            private int _move = 0;
            public int Move
            {
                get => _move;
                set
                {
                    if(_move != value)
                    {
                        _move = value;
                        UIManager.Instance.MoveNumText.text = Move.ToString();
                    }
                }
            }

            [SerializeField]
            private int[] _colorBlockNum = new int[5];
            public int[] ColorBlockNum
            {
                get => _colorBlockNum;
                set => _colorBlockNum = value;
            }
            public int MaxColorBlockIndex
            {
                get
                {
                    int num = int.MinValue;
                    int index = 0;

                    for(int i = 0; i < ColorBlockNum.Length; ++i)
                    {
                        if(IsExistRunningRainbowEffect(i))
                        {
                            continue;
                        }
                        if(num > ColorBlockNum[i])
                        {
                            index = i;
                        }
                    }

                    return index;
                }
            }

            [SerializeField]
            private int _waitingSwapNum = 0;
            public void AddWaitingSwapNum() => ++_waitingSwapNum;
            public void ReduceWaitingSwapNum() => --_waitingSwapNum;
            public bool IsWaitingSwap => _waitingSwapNum > 0;

            [SerializeField]
            private List<int> _runningRainbowEffect = new List<int>();
            public void AddRunningRainbowEffect(int colorIndex)
            {
                if (!IsExistRunningRainbowEffect(colorIndex))
                {
                    _runningRainbowEffect.Add(colorIndex);
                }
            }
            public void RemoveRunningRainbowEffect(int colorIndex)
            {
                _runningRainbowEffect.Remove(colorIndex);
            }
            public bool IsExistRunningRainbowEffect(int colorIndex)
            {
                return _runningRainbowEffect.IndexOf(colorIndex) != -1;
            }

            #endregion

            #region Grid data

            [SerializeField]
            private GridData _grid;
            public GridData Grid => _grid;

            [SerializeField]
            private GameObject _prefabGrid;

            public Vector2 GridPosition
            {
                get
                {
                    if(Grid == null)
                    {
                        return ConstantData.POS_ERROR_VALUE;
                    }
                    return Grid.TrGrid.position;
                }
            }

            #endregion

            #region Cell data

            public CellData GetCell(Vector2Int pos)
            {
                if(Grid == null)
                {
                    return null;
                }

                return Grid.GetCell(pos);
            }

            #endregion

            #region Game state

            [System.Flags]
            public enum GameState
            {
                Idle = 0x00000000,

                // allocate 4bit (0x0000000F)
                // game initalize state
                GameStart = 0x00000001,
                Loading_GridData = 0x00000002,
                Loading_GridMask = 0x00000004,
                Loading_GirdBoard = 0x00000008,

                // allocate 4 bit (0x000000F0)
                // game play state
                Processing_UserInput = 0x00000010,
                Stop_Checking = 0x00000020,
                Done_GameEnd = 0x00000040,
            }

            [SerializeField]
            private GameState _state = GameState.GameStart;
            public GameState State => _state;

            public void AddGameState(GameState state)
            {
                _state |= state;
            }

            public void RemoveGameState(GameState state)
            {
                _state &= ~state;
            }

            public bool IsContainState(GameState state)
            {
                return (_state & state) == state;
            }

            #endregion

            #region Game time

            public int _gameTimeVersion = 0;

            public GameTimeData GameTime
            {
                get
                {
                    return new GameTimeData()
                    {
                        DeltaTime = (int)Time.realtimeSinceStartup,
                        Version = _gameTimeVersion
                    };
                }
            }

            #endregion

            #region General

            private void Start()
            {
                InputManager.Instance.SetTouchAction(InputTouch);
                StartCoroutine(GameStart());
            }

            IEnumerator GameStart()
            {
                yield return null;

                Application.targetFrameRate = 120;

                LoadData();
            }

            #endregion

            #region Data load

            private void LoadData()
            {
                RemoveGameState(GameState.GameStart);
                AddGameState(GameState.Loading_GridData);
                TextAsset mapFile = Resources.Load<TextAsset>("LevelData/Sample");
                if(mapFile == null)
                {
                    return;
                }
                JsonData root = JsonMapper.ToObject(mapFile.text);

                // Move load
                Move = InGameUtil.ParseInt(ref root, ConstantData.MAP_KEY_MOVE, 0);

                // Grid load
                LitJson.JsonData gridRoot = root[ConstantData.MAP_KEY_GRID];
                _grid = Instantiate(_prefabGrid, _trLevel).GetComponent<GridData>();
                _grid.transform.localPosition = Vector3.zero;
                _grid.LoadGridData(gridRoot);

                MissionManager.Instance.LoadMissions(gridRoot[ConstantData.MAP_KEY_MISSION_LIST]);
                RemoveGameState(GameState.Loading_GridData);
            }

            #endregion

            #region Input

            private Vector2Int _targetCellPos = CellIndex.None;
            private Vector2 _touchDownPoisition;

            private Vector2Int TouchPosConvertToCellPos(Vector2 touchPos)
            {
                Vector2 touchPositionOverGrid = touchPos - GridPosition - new Vector2(-4.5f, -5.5f);
                if (touchPositionOverGrid.x >= 0 && touchPositionOverGrid.y >= 0)
                {
                    int x = (int)touchPositionOverGrid.x;
                    int y = (int)touchPositionOverGrid.y;
                    if (x >= 0 && x < ConstantData.MAX_GRID_WIDTH_SIZE && y >= 0 && y < ConstantData.MAX_GRID_HEIGHT_SIZE)
                    {
                        if (GetCell(new Vector2Int(x, y)) == null || GetCell(new Vector2Int(x, y)).IsEmptyCell)
                        {
                            return CellIndex.None;
                        }
                        return new Vector2Int(x, y);
                    }
                }
                return CellIndex.None;
            }

            private void InputTouch(Vector2 touchPosition, TouchPhase type)
            {
                Vector2Int cellPosition = TouchPosConvertToCellPos(touchPosition);
                if (cellPosition == CellIndex.None)
                {
                    _targetCellPos = CellIndex.None;
                    return;
                }

                if(IsContainState(GameState.Processing_UserInput) || 
                    IsContainState(GameState.Loading_GridData) ||
                    IsContainState(GameState.GameStart)||
                    IsContainState(GameState.Done_GameEnd))
                {
                    _targetCellPos = CellIndex.None;
                    return;
                }

                switch (type)
                {
                    case TouchPhase.Began:
                    {
                        OnTouchDown(cellPosition, touchPosition);
                        break;
                    }
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                    {
                        OnTouchUp(cellPosition, touchPosition);
                        break;
                    }
                    case TouchPhase.Moved:
                    {
                        OnTouchDrag(cellPosition, touchPosition);
                        break;
                    }
                }
            }

            public void OnTouchDown(Vector2Int cellPosition, Vector2 touchPosition)
            {
                _targetCellPos = cellPosition;
                _touchDownPoisition = touchPosition;
            }

            public void OnTouchUp(Vector2Int cellPosition, Vector2 touchPosition)
            {
                if(_targetCellPos == CellIndex.None)
                {
                    return;
                }
                if(_targetCellPos != cellPosition)
                {
                    OnTouchDrag(cellPosition, touchPosition);
                    return;
                }

                //OnUsetInput();
                _targetCellPos = CellIndex.None;
            }

            public void OnTouchDrag(Vector2Int cellPosition, Vector2 touchPosition)
            {
                if(_targetCellPos == CellIndex.None)
                {
                    return;
                }
                if(_targetCellPos == cellPosition)
                {
                    return;
                }

                Vector2 diffPosition = touchPosition - _touchDownPoisition;
                bool isPositiveX = diffPosition.x > 0;
                bool isPositiveY = diffPosition.y > 0;
                float absX = isPositiveX ? diffPosition.x : -diffPosition.x;
                float absY = isPositiveY ? diffPosition.y : -diffPosition.y;

                Vector2Int direction = Vector2Int.zero;
                if (absX > absY)
                {
                    if (isPositiveX)
                    {
                        direction = CellIndex.RightDirection;
                    }
                    else
                    {
                        direction = CellIndex.LeftDirection;
                    }
                }
                else
                {
                    if (isPositiveY)
                    {
                        direction = CellIndex.UpDirection;
                    }
                    else
                    {
                        direction = CellIndex.DownDirection;
                    }
                }

                if(!Grid.VerificationSwap(_targetCellPos, direction))
                {
                    _targetCellPos = CellIndex.None;
                    return;
                }

                Grid.InputSwap(_targetCellPos, direction);
                OnUsetInput();
                _targetCellPos = CellIndex.None;
            }

            public void OnUsetInput()
            {
                AddGameState(GameState.Processing_UserInput);
                ++_gameTimeVersion;
            }

            #endregion

        }
    }
}
