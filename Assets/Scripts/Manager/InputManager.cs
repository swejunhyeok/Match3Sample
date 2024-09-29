using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {
        public class InputManager : SingletonManager<InputManager>
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

            #region Unity Component

            [Header("- Unity Component -")]
            [SerializeField]
            private Camera _cameraMain = null;

            #endregion

            #region General

            private void Update()
            {
                TouchFunc();
            }

            #endregion

            #region Touch func

            private System.Action<Vector2, TouchPhase> _touchAction = null;

            public void SetTouchAction(System.Action<Vector2, TouchPhase> action) => _touchAction = action;

            private void TouchFunc()
            {
                if(_cameraMain == null)
                {
                    _cameraMain = Camera.main;
                }

                if(_cameraMain == null)
                {
                    return;
                }

                if (Input.GetMouseButtonDown(0))
                { 
                    _touchAction?.Invoke(_cameraMain.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Began);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    _touchAction?.Invoke(_cameraMain.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Ended);
                }
                else if (Input.GetMouseButton(0))
                {
                    _touchAction?.Invoke(_cameraMain.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Moved);
                }
                return;
            }

            #endregion

        }
    }
}
