using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {
        public class GridData : Match3Data
        {

            #region Cell data

            private CellData[,] _cells = new CellData[(ConstantData.MAX_GRID_HEIGHT_SIZE + 1), ConstantData.MAX_GRID_WIDTH_SIZE];
            
            public CellData GetCell(Vector2Int pos)
            {
                return GetCell(pos.x, pos.y);
            }
            
            public CellData GetCell(int x, int y)
            {
                if(_cells == null)
                {
                    return null;
                }
                if(x < 0 || x > _cells.GetLength(1))
                {
                    return null;
                }
                if(y < 0 || y > _cells.GetLength(0))
                {
                    return null;
                }
                return _cells[y, x];
            }

            #endregion



        }
    }
}
