using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {
        public class ObjectPoolManager : SingletonManager<ObjectPoolManager>
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
                else
                {
                    DontDestroyOnLoad(gameObject);

                    InitObjectPool();
                }
            }

            #endregion

            #region Prefab

            [Header("- Prefab -")]
            [SerializeField]
            private GameObject _prefabCell;
            [SerializeField]
            private GameObject _prefabColorBlock;
            [SerializeField]
            private GameObject _prefabSpecialBlock;
            [SerializeField]
            private GameObject _prefabGimmickBlock;

            #endregion

            #region Object pool

            private ObjectPool<CellData> _objectPoolCell = new ObjectPool<CellData>();
            public CellData GetCellData(Transform parent = null)
            {
                CellData cellData = _objectPoolCell.GetObject();
                cellData.gameObject.SetActive(true);
                if(parent != null)
                {
                    cellData.transform.parent = parent;
                }
                return cellData;
            }
            public void Dispose(CellData cell)
            {
                _objectPoolCell.Dispose(cell);
            }

            private ObjectPool<BlockData> _objectPoolColorBlock = new ObjectPool<BlockData>();
            private ObjectPool<BlockData> _objectPoolSpecialBlock = new ObjectPool<BlockData>();
            private ObjectPool<BlockData> _objectPoolGimmickBlock = new ObjectPool<BlockData>();

            public BlockData GetObjectData(BlockKind kind, Transform parent = null)
            {
                BlockData block = null;
                switch(kind)
                {
                    case BlockKind.ColorBlock:
                    {
                        block = _objectPoolColorBlock.GetObject();
                        break;
                    }
                    case BlockKind.SpecialBlock:
                    {
                        block = _objectPoolSpecialBlock.GetObject();
                        break;
                    }
                    case BlockKind.GimmickBlock:
                    {
                        block = _objectPoolGimmickBlock.GetObject();
                        break;
                    }
                }

                if(block == null)
                {
                    if(Debug.isDebugBuild)
                    {
                        Debug.Log($"Block is null. kind = {kind}");
                    }
                    return block;
                }

                block.gameObject.SetActive(true);
                if(parent != null)
                {
                    block.transform.parent = parent;
                }
                return block;
            }
            public void Dispose(BlockData block)
            {
                block.transform.position = new Vector2(int.MaxValue, int.MaxValue);
                if(block.Attribute == null)
                {
                    block.gameObject.SetActive(false);
                    return;
                }
                switch(block.Attribute.Kind)
                {
                    case BlockKind.ColorBlock:
                    {
                        _objectPoolColorBlock.Dispose(block);
                        break;
                    }
                    case BlockKind.SpecialBlock:
                    {
                        _objectPoolSpecialBlock.Dispose(block);
                        break;
                    }
                    case BlockKind.GimmickBlock:
                    {
                        _objectPoolGimmickBlock.Dispose(block);
                        break;
                    }
                }
            }

            #endregion

            #region General

            private void InitObjectPool()
            {
                _objectPoolCell.Init(_prefabCell, transform);
                _objectPoolColorBlock.Init(_prefabColorBlock, transform);
                _objectPoolSpecialBlock.Init(_prefabSpecialBlock, transform);
                _objectPoolGimmickBlock.Init(_prefabGimmickBlock, transform);
            }

            #endregion

        }
    }
}
