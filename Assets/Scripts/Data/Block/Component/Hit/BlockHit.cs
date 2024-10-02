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

            protected void RunHitEffect(HitConditionType hitCondition, BlockType hitBlock)
            {
                bool isDestroy = false;
                BlockAttribute changeAttribute = null;
                if((Block.Attribute.HitEffect & HitEffectType.Change) == HitEffectType.Change)
                {
                    changeAttribute = Block.Attribute.HitChangeGimmick;
                }
                if((Block.Attribute.HitEffect & HitEffectType.ReduceMission) == HitEffectType.ReduceMission)
                {
                    MissionManager.Instance.ReduceMission(Block.Attribute.Mission);
                }
                if((Block.Attribute.HitEffect & HitEffectType.Destroy) == HitEffectType.Destroy)
                {
                    isDestroy = true;
                }
                if((Block.Attribute.HitEffect & HitEffectType.ArroundHit) == HitEffectType.ArroundHit)
                {
                    ArroundHit(hitCondition, hitBlock);
                }
                if(isDestroy)
                {
                    Destroy();
                }
                if(changeAttribute != null)
                {
                    Block.ChangeAttribute(changeAttribute);
                }
            }

            protected void ArroundHit(HitConditionType hitCondition, BlockType hitBlock)
            {
                if(hitCondition == HitConditionType.Powerup && hitBlock != BlockType.Rainbow)
                {
                    return;
                }
                LayerType hitLayer = LayerType.Bottom | LayerType.Middle | LayerType.Top;
                foreach(CellData cell in Block.PivotCell.FourDirectionCell)
                {
                    if(cell == null)
                    {
                        continue;
                    }
                    if(Block.Cache.HitPositions != null && Block.Cache.HitPositions.IndexOf(cell.Pos) != -1)
                    {
                        continue;
                    }
                    cell.Block.Hit(HitConditionType.ArroundMatch, hitLayer, Block.Attribute.Type, Block.Cache.HitBlocks, Block.Cache.HitPositions);
                    if(Block.Cache.HitPositions != null)
                    {
                        Block.Cache.HitPositions.Add(cell.Pos);
                    }
                }
            }

            public void Hit(
                HitConditionType hitCondition,
                BlockType hitBlock,
                System.Action successCallback = null)
            {
                if(!CheckHitCondition(hitCondition))
                {
                    return;
                }

                RunHitEffect(hitCondition, hitBlock);
                successCallback?.Invoke();
            }

            private void Destroy()
            {

                Block.RemoveCell();
                Block.Dispose();
            }


            #endregion

        }
    }
}
