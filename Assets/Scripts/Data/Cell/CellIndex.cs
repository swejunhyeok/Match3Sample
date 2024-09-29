using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace JH
{
    namespace Match3Sample
    {
        public class CellIndex
        {

            #region Static value

            public static Vector2Int None = new Vector2Int(int.MinValue, int.MinValue);

            public static Vector2Int UpDirection = new Vector2Int(0, 1);
            public static Vector2Int DownDirection = new Vector2Int(0, -1);
            public static Vector2Int LeftDirection = new Vector2Int(-1, 0);
            public static Vector2Int RightDirection = new Vector2Int(1, 0);

            public static Vector2Int UpLeftDirection = UpDirection + LeftDirection;
            public static Vector2Int UpRightDirection = UpDirection + RightDirection;
            public static Vector2Int DownLeftDirection = DownDirection + LeftDirection;
            public static Vector2Int DownRightDirection = DownDirection + RightDirection;

            public static Vector2Int[] FourDirection = new Vector2Int[4] { UpDirection, DownDirection, LeftDirection, RightDirection };
            public static int UpIndex = 0;
            public static int DownIndex = 1;
            public static int LeftIndex = 2;
            public static int RightIndex = 3;
            public static Vector2Int[] FourDiagonalDirection = new Vector2Int[4] { UpLeftDirection, UpRightDirection, DownLeftDirection, DownRightDirection };
            public static int UpLeftIndex = 0;
            public static int UpRightIndex = 1;
            public static int DownLeftIndex = 2;
            public static int DownRightIndex = 3;

            #endregion

            #region Utils

            public static bool Verification(Vector2Int pivot, Vector2Int direction, bool hasGenerateCell = false)
            {
                return Verification(pivot + direction, hasGenerateCell);
            }

            public static bool Verification(Vector2Int position, bool hasGenerateCell = false)
            {
                if (position.x < 0 || position.x >= ConstantData.MAX_GRID_WIDTH_SIZE)
                {
                    return false;
                }
                if (position.y < 0 || position.y >= ConstantData.MAX_GRID_HEIGHT_SIZE + (hasGenerateCell ? 1 : 0))
                {
                    return false;
                }
                return true;
            }

            public static Vector2Int IndexConvertToPos(int index)
            {
                return new Vector2Int(index % ConstantData.MAX_GRID_WIDTH_SIZE, index / ConstantData.MAX_GRID_WIDTH_SIZE);
            }

            #endregion

        }
    }
}
