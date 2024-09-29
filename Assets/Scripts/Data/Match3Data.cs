using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {
        public class Match3Data : MonoBehaviour
        {

            #region General

            public virtual void Init()
            {
                gameObject.SetActive(true);
            }

            public virtual void Dispose(bool isForce = false)
            {

            }

            #endregion

        }
    }
}
