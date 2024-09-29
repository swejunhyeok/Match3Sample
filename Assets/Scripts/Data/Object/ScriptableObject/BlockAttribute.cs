using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {

        public enum BlockType
        {
            None = 0,

            ___ColorBlock___ = 1,
            BlockRed,
            BlockBlue,
            BlockYellow,
            BlockGreen,
            BlockPurple,

            ___SpecialBlock___ = 1001,


            ___Gimmick___ = 2001,
            Gimmick_1,
            Gimmick_2,
            Gimmick_3,
            Gimmick_4
        }

        public enum BlockKind
        {
            None,
            ColorBlock,
            SpecialBlock,
            GimmickBlock,
        }

        [System.Flags]
        public enum LayerType
        {
            None = 0x000,
            Board = 0x001,
            Bottom = 0x002,
            Middle = 0x004,
            Top = 0x008,
        }

        public enum ColorType
        {
            None,
            Red,
            Blue,
            Yellow,
            Green,
            Purple,
        }

        public enum MissionType
        {
            None,
            PaperBox
        }

        [System.Flags]
        public enum HitConditionType
        {
            None = 0x000,
            ArroundMatch = 0x001,
            InnerMatch = 0x002,
            Powerup = 0x004,
            Match = 0x008,
            Touch = 0x010,
        }

        [System.Flags]
        public enum HitEffectType
        {
            None = 0x000,
            Change = 0x001,
            ReduceMission = 0x002,
            Destroy = 0x004,
        }

        [CreateAssetMenu(fileName = "Block Attribute", menuName = "JH/Match3Sample/Block Attribute")]
        public class BlockAttribute : ScriptableObject
        {
            #region Common

            /// <summary>
            /// Type of block.
            /// </summary>
            [SerializeField]
            private BlockType _type;
            public BlockType Type
            {
                get => _type;
            }

            [SerializeField]
            private BlockKind _kind;
            public BlockKind Kind
            {
                get => _kind;
            }

            [SerializeField]
            private LayerType _layer;
            public LayerType Layer
            {
                get => _layer;
            }

            [SerializeField]
            private Vector2Int _size;
            public Vector2Int Size
            {
                get => _size;
            }

            [SerializeField]
            private int _health;
            public int Health
            {
                get => _health;
            }

            [SerializeField]
            private ColorType _color;
            public ColorType Color
            {
                get => _color;
            }

            [SerializeField]
            private Sprite _sprBlock;
            public Sprite SprBlock
            {
                get => _sprBlock;
            }

            #endregion

            #region Layer properties

            public bool IsBottomLayer => LayerEqauls(Layer, LayerType.Bottom);
            public bool IsMiddleLayer => LayerEqauls(Layer, LayerType.Middle);
            public bool IsTopLayer => LayerEqauls(Layer, LayerType.Top);

            private bool LayerEqauls(LayerType main, LayerType target)
            {
                return (main & target) == target;
            }

            #endregion

            #region State

            [SerializeField]
            private bool _isMatchAble;
            public bool IsMatchAble
            {
                get => _isMatchAble;
            }

            [SerializeField]
            private bool _isMoveAble;
            public bool IsMoveAble
            {
                get => _isMoveAble;
            }

            #endregion

            #region Hit

            [SerializeField]
            private HitConditionType _hitCondition;
            public HitConditionType HitCondition
            {
                get => _hitCondition;
            }

            [SerializeField]
            private HitEffectType _hitEffect;
            public HitEffectType HitEffect
            {
                get => _hitEffect;
            }

            [SerializeField]
            private MissionType _mission;
            public MissionType Mission
            {
                get => _mission;
            }

            [SerializeField]
            private BlockAttribute _hitChangeGimmick;
            public BlockAttribute HitChangeGimmick
            {
                get => _hitChangeGimmick;
            }

            #endregion

        }
    }
}
