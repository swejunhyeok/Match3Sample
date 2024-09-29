using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {
        public class Mission
        {

            [SerializeField]
            private MissionType _type;

            [SerializeField]
            private int _num;

            private bool _isClear = false;


            public void Init(JsonData missionRoot)
            {
                _type = (MissionType)InGameUtil.ParseInt(ref missionRoot, ConstantData.MAP_KEY_MISSION_TYPE, 0);
                _num = InGameUtil.ParseInt(ref missionRoot, ConstantData.MAP_KEY_MISSION_NUM, 0);
            }

        }
    }
}
