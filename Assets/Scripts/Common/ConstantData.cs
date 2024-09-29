using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {
        public static class ConstantData
        {

            #region Game

            public static Vector2Int MAX_GRID_SIZE = new Vector2Int(9, 11);
            public static int MAX_GRID_WIDTH_SIZE => MAX_GRID_SIZE.x;
            public static int MAX_GRID_HEIGHT_SIZE => MAX_GRID_SIZE.y;

            public static Vector2 POS_ERROR_VALUE = new Vector2(int.MinValue, int.MinValue);



            #endregion

        }
    }
}
