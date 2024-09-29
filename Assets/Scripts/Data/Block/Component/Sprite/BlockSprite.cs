using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {
        public class BlockSprite : BlockComponent
        {

            #region Unity component

            [SerializeField]
            private SpriteRenderer _sprRenderer;

            public void SetSprite(Sprite spr)
            {
                _sprRenderer.sprite = spr;
            }

            #endregion

        }
    }
}
