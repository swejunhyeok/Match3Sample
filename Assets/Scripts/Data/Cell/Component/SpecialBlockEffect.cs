using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;
using static UnityEditor.PlayerSettings;

namespace JH
{
    namespace Match3Sample
    {
        [System.Flags]
        public enum SpecialBlockEffectType
        {
            None = 0x001,
            Prop = 0x002,
            RocketTransverse = 0x004,
            RocketVertical = 0x008,
            Bomb = 0x010,
            Rainbow = 0x020,
            Double = 0x040,
            ColorRed = 0x080,
            ColorBlue = 0x100,
            ColorGreen = 0x200,
            ColorYellow = 0x400,
            ColorPurple = 0x800,
        }

        public class SpecialBlockEffect : CellComponent
        {
            [SerializeField]
            private bool _isReservation = false;

            [SerializeField]
            private SpecialBlockEffectType _effect = SpecialBlockEffectType.None;

            [SerializeField]
            private List<Vector2Int> _hitPosition = null;

            [SerializeField]
            private List<BlockData> _hitBlock = null;

            public void SetReservation(SpecialBlockEffectType effect)
            {
                _isReservation = true;
                _effect = effect;
            }

            public void Run(BlockType type)
            {
                switch(type)
                {
                    case BlockType.Prop:
                    {
                        Run(SpecialBlockEffectType.Prop);
                        break;
                    }
                    case BlockType.RocketTransverse:
                    {
                        Run(SpecialBlockEffectType.RocketTransverse);
                        break;
                    }
                    case BlockType.RocketVertical:
                    {
                        Run(SpecialBlockEffectType.RocketVertical);
                        break;
                    }
                    case BlockType.Bomb:
                    {
                        Run(SpecialBlockEffectType.Bomb);
                        break;
                    }
                    case BlockType.Rainbow:
                    {
                        Run(SpecialBlockEffectType.Rainbow);
                        break;
                    }
                }
            }

            public void Run(SpecialBlockEffectType effect)
            {
                SetHitList(new List<Vector2Int>(), new List<BlockData>());
                if((effect & SpecialBlockEffectType.RocketTransverse) == SpecialBlockEffectType.RocketTransverse)
                {
                    effect &= SpecialBlockEffectType.RocketTransverse;
                    switch(effect)
                    {
                        case SpecialBlockEffectType.None:
                        {
                            break;
                        }
                    }
                }
            }

            private void SetHitList(List<Vector2Int> hitPosition = null, List<BlockData> hitBlock = null)
            {
                if (hitPosition != null)
                {
                    _hitPosition = hitPosition;
                }
                if (hitBlock != null)
                {
                    _hitBlock = hitBlock;
                }
            }

            private void Hit(Vector2Int pos, LayerType layer, BlockType type, bool isHitOnce = false)
            {
                if(!CellIndex.Verification(pos))
                {
                    return;
                }
                if(isHitOnce && _hitPosition.IndexOf(pos) != -1)
                {
                    return;
                }
                CellData pivotCell = GameManager.Instance.GetCell(pos);
                pivotCell.Block.Hit(HitConditionType.Powerup, layer, type, _hitBlock, _hitPosition);
                if(isHitOnce)
                {
                    _hitPosition.Add(pos);
                }
            }

            #region Single special block effect

            private void RocketTransverseEffect()
            {
                Vector2Int pivotPos = Cell.Pos;
                for(int x = 0; x < ConstantData.MAX_GRID_WIDTH_SIZE; ++x)
                {
                    Vector2Int targetPos = pivotPos + new Vector2Int(x, 0);
                    Hit(targetPos, LayerType.Everything, BlockType.RocketTransverse, true);
                    targetPos = pivotPos + new Vector2Int(-x, 0);
                    Hit(targetPos, LayerType.Everything, BlockType.RocketTransverse, true);
                }
            }

            private void RocketVerticalEffect()
            {
                Vector2Int pivotPos = Cell.Pos;
                for (int y = 0; y < ConstantData.MAX_GRID_HEIGHT_SIZE; ++y)
                {
                    Vector2Int targetPos = pivotPos + new Vector2Int(0, y);
                    Hit(targetPos, LayerType.Everything, BlockType.RocketVertical, true);
                    targetPos = pivotPos + new Vector2Int(0, -y);
                    Hit(targetPos, LayerType.Everything, BlockType.RocketVertical, true);
                }
            }

            private void BombEffect(bool isDouble)
            {
                Vector2Int pivotPos = Cell.Pos;
                int startIndex = isDouble ? (-4) : (-2);
                int endIndex = isDouble ? 5 : 3;
                for(int x = startIndex; x < endIndex; ++x)
                {
                    for(int y = startIndex; y < endIndex; ++y)
                    {
                        Vector2Int targetPos = pivotPos + new Vector2Int(x, y);
                        Hit(targetPos, LayerType.Everything, BlockType.Bomb, true);
                    }
                }
            }

            private void RainbowEffect(ColorType type)
            {
                if(type == ColorType.None)
                {
                    type = (ColorType)(GameManager.Instance.MaxColorBlockIndex + 1);
                }

                for(int y = 0; y < ConstantData.MAX_GRID_HEIGHT_SIZE; ++y)
                {
                    for(int x = 0; x < ConstantData.MAX_GRID_WIDTH_SIZE; ++x)
                    {
                        Vector2Int pos = new Vector2Int(x, y); 
                        CellData cell = GameManager.Instance.GetCell(pos);
                        if(cell == null || !cell.IsVisibleCell)
                        {
                            continue;
                        }
                        if(!cell.Block.HasMiddleBlock)
                        {
                            continue;
                        }
                        if(!cell.Block.MiddleBlock.HasAttribute)
                        {
                            continue;
                        }
                        if(cell.Block.MiddleBlock.Attribute.Color != type)
                        {
                            continue;
                        }
                        Hit(pos, LayerType.Middle, BlockType.Rainbow, true);
                    }
                }
            }

            #endregion

            #region Mix special block effect

            private void RocketCrossEffect()
            {
                Vector2Int pivotPos = Cell.Pos;
                for (int i = 0; i < ConstantData.MAX_GRID_HEIGHT_SIZE; ++i)
                {
                    Vector2Int targetPos = pivotPos + new Vector2Int(i, 0);
                    Hit(targetPos, LayerType.Everything, BlockType.RocketTransverse, true);
                    targetPos = pivotPos + new Vector2Int(-i, 0);
                    Hit(targetPos, LayerType.Everything, BlockType.RocketTransverse, true);
                    targetPos = pivotPos + new Vector2Int(0, i);
                    Hit(targetPos, LayerType.Everything, BlockType.RocketVertical, true);
                    targetPos = pivotPos + new Vector2Int(0, -i);
                    Hit(targetPos, LayerType.Everything, BlockType.RocketVertical, true);
                }
            }

            private void RocketWideCrossEffect()
            {
                Vector2Int pivotPos = Cell.Pos;
                for (int i = 0; i < ConstantData.MAX_GRID_HEIGHT_SIZE; ++i)
                {
                    for (int j = -1; j < 2; ++j)
                    {
                        Vector2Int targetPos = pivotPos + new Vector2Int(i, j);
                        Hit(targetPos, LayerType.Everything, BlockType.RocketTransverse, true);
                        targetPos = pivotPos + new Vector2Int(-i, j);
                        Hit(targetPos, LayerType.Everything, BlockType.RocketTransverse, true);
                        targetPos = pivotPos + new Vector2Int(j, i);
                        Hit(targetPos, LayerType.Everything, BlockType.RocketVertical, true);
                        targetPos = pivotPos + new Vector2Int(j, -i);
                        Hit(targetPos, LayerType.Everything, BlockType.RocketVertical, true);
                    }
                }
            }

            private void MixRainbowEffect(BlockType changeType)
            {
                ColorType colorType = (ColorType)(GameManager.Instance.MaxColorBlockIndex + 1);
                List<Vector2Int> changePosition = new List<Vector2Int>();
                for (int y = 0; y < ConstantData.MAX_GRID_HEIGHT_SIZE; ++y)
                {
                    for (int x = 0; x < ConstantData.MAX_GRID_WIDTH_SIZE; ++x)
                    {
                        Vector2Int pos = new Vector2Int(x, y);
                        CellData cell = GameManager.Instance.GetCell(pos);
                        if (cell == null || !cell.IsVisibleCell)
                        {
                            continue;
                        }
                        if (!cell.Block.HasMiddleBlock)
                        {
                            continue;
                        }
                        if (!cell.Block.MiddleBlock.HasAttribute)
                        {
                            continue;
                        }
                        if (cell.Block.MiddleBlock.Attribute.Color != colorType)
                        {
                            continue;
                        }
                        Hit(pos, LayerType.Middle, BlockType.Rainbow);
                        if(changeType == BlockType.RocketVertical || changeType == BlockType.RocketTransverse)
                        {
                            changeType = Random.Range(0, 2) == 1 ? BlockType.RocketTransverse : BlockType.RocketVertical;
                        }
                        cell.Block.CreateBlock(BlockKind.SpecialBlock, changeType);
                        changePosition.Add(pos);
                    }
                }
                for(int i = 0; i < changePosition.Count; ++i)
                {
                    Hit(changePosition[i], LayerType.Middle, BlockType.Rainbow, true);
                }
            }

            private void DoubleRainbowEffect()
            {
                Vector2Int pivotPos = Cell.Pos;
                Hit(pivotPos, LayerType.Everything, BlockType.Rainbow, true);
                for (int i = 1; i < ConstantData.MAX_GRID_HEIGHT_SIZE; ++i)
                {
                    int pivotX = i;
                    for(int j = -pivotX; j <= pivotX; ++j)
                    {
                        for(int k = 0; k < 2; ++k)
                        {
                            Vector2Int targetPos = pivotPos + new Vector2Int(pivotX * (k == 0? 1 : -1), j);
                            Hit(targetPos, LayerType.Everything, BlockType.Rainbow, true);
                        }
                    }
                    for(int j = -pivotX + 1; j <= pivotX - 1; ++j)
                    {
                        for(int k = 0; j < 2; ++k)
                        {
                            Vector2Int targetPos = pivotPos + new Vector2Int(j, pivotX * (k == 0 ? 1 : -1));
                            Hit(targetPos, LayerType.Everything, BlockType.Rainbow, true);
                        }
                    }
                }
            }

            #endregion

        }
    }
}
