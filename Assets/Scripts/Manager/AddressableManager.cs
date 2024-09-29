using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace JH
{
    namespace Match3Sample
    {
        public class AddressableManager : SingletonManager<AddressableManager>
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
                }
            }

            #endregion

            #region Block attribute

            private Dictionary<int, BlockAttribute> _dicBlockAttributes = new Dictionary<int, BlockAttribute>();
            public BlockAttribute GetBlockAttribute(BlockType type)
            {
                int typeId = (int)type;
                if(_dicBlockAttributes.ContainsKey(typeId))
                {
                    return _dicBlockAttributes[typeId];
                }

                var op = Addressables.LoadAssetAsync<BlockAttribute>(type.ToString());
                BlockAttribute BlockAttribute = op.WaitForCompletion();

                _dicBlockAttributes.Add(typeId, BlockAttribute);

                return BlockAttribute;
            }
            #endregion

        }
    }
}
