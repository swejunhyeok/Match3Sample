using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {
        public class GenerateCell : CellComponent
        {
            [System.Serializable]
            private struct GenerateData
            {
                public BlockType Type;
                public int Weight;
            }

            [SerializeField]
            private GenerateData[] _generateDatas = null;

            private int _totalWeight = 0;

            public void LoadGenerateData(LitJson.JsonData generatesRoot)
            {
                _generateDatas = new GenerateData[generatesRoot.Count];

                for(int i = 0; i < generatesRoot.Count; ++i)
                {
                    LitJson.JsonData generateRoot = generatesRoot[i];
                    _generateDatas[i] = new GenerateData()
                    {
                        Type = (BlockType)InGameUtil.ParseInt(ref generateRoot, ConstantData.MAP_KEY_BLOCK_TYPE, 0),
                        Weight = InGameUtil.ParseInt(ref generateRoot, ConstantData.MAP_KEY_CELL_WEIGHT, 0)
                    };
                    _totalWeight += _generateDatas[i].Weight;
                }
            }

            private BlockType GetBlockType()
            {
                int randomValue = Random.Range(0, _totalWeight);

                int selectIndex = -1;
                int summaryValue = 0;

                for(int i = 0; i < _generateDatas.Length; ++i)
                {
                    summaryValue += _generateDatas[i].Weight;
                    if(randomValue < summaryValue)
                    {
                        selectIndex = i;
                        break;
                    }
                }

                if(selectIndex == -1)
                {
                    if(Debug.isDebugBuild)
                    {
                        Debug.LogError("Generate data is error.");
                    }
                    return BlockType.None;
                }
                return _generateDatas[selectIndex].Type;
            }

            public void GenerateObject()
            {
                if(Cell.Block.HasMiddleBlock)
                {
                    return;
                }
                BlockType type = GetBlockType();
                Cell.Block.CreateBlock(AddressableManager.Instance.GetBlockAttribute(type).Kind, type);
            }
        }
    }
}
