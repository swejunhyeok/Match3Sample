using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JH
{
    namespace Match3Sample
    {
        public class UIManager : SingletonManager<UIManager>
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
            }

            #endregion

            [SerializeField]
            private Text _moveNumText;
            public Text MoveNumText => _moveNumText;

            [SerializeField]
            private MissionUI _missionUI;
            public MissionUI MissionUI => _missionUI;
        }
    }
}
