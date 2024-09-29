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

            #region Gimmick attribute

            private Dictionary<int, BlockAttribute> _dicGimmickAttributes = new Dictionary<int, BlockAttribute>();
            public BlockAttribute GetGimmickAttribute(BlockType type)
            {
                int typeId = (int)type;
                if(_dicGimmickAttributes.ContainsKey(typeId))
                {
                    return _dicGimmickAttributes[typeId];
                }

                var op = Addressables.LoadAssetAsync<BlockAttribute>(type.ToString());
                BlockAttribute gimmickAttribute = op.WaitForCompletion();

                _dicGimmickAttributes.Add(typeId, gimmickAttribute);

                return gimmickAttribute;
            }
            #endregion

        }
    }
}
