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

            #region Map

            public static string MAP_KEY_MOVE = "move";
            public static string MAP_KEY_GRID = "grid";

            // mission
            public static string MAP_KEY_MISSION_LIST = "msl";
            public static string MAP_KEY_MISSION_TYPE = "mst";
            public static string MAP_KEY_MISSION_NUM = "msn";

            // cell
            public static string MAP_KEY_CELL_LIST = "cs";
            public static string MAP_KEY_CELL_TYPE = "ct";
            public static string MAP_KEY_CELL_GENERATE_LIST = "gl";
            public static string MAP_KEY_CELL_WEIGHT = "we";

            // block
            public static string MAP_KEY_BLOCK_LIST = "bts";
            public static string MAP_KEY_BLOCK_TYPE = "bt";

            #endregion

        }
    }
}
