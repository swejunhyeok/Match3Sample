using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    public class SingletonManager<T> : MonoBehaviour where T : MonoBehaviour
    {

        #region Instance

        protected static T _instance = null;

        public static T Instance
        {
            get
            {
                return _instance;
            }
        }

        protected static void Init()
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<T>();
            }

            if(_instance == null)
            {
                GameObject goSingleton = new GameObject(typeof(T).Name);
                goSingleton.AddComponent<T>();
                _instance = goSingleton.GetComponent<T>();
            }
        }

        #endregion

    }
}
