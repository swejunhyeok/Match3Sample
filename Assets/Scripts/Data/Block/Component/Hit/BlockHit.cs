using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {

        public class BlockHit : BlockComponent
        {

            #region Hit

            protected bool CheckHitCondition(HitConditionType hitCondition)
            {
                if(!Block.HasAttribute)
                {
                    return false;
                }
                return (Block.Attribute.HitCondition & hitCondition) == hitCondition;
            }



            #endregion

        }
    }
}
