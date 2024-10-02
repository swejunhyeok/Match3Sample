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
            public MissionType Type => _type;

            [SerializeField]
            private int _num = 0;
            private int Num
            {
                get => _num;
                set
                {
                    if(_num != value)
                    {
                        _num = value;
                        UIManager.Instance.MissionUI.TxtMissionNum.text = _num.ToString();
                    }
                }
            }

            [SerializeField]
            private MissionAttribute _attribute;

            private bool _isClear = false;
            public bool IsClear => _isClear;

            public void Init(JsonData missionRoot)
            {
                _type = (MissionType)InGameUtil.ParseInt(ref missionRoot, ConstantData.MAP_KEY_MISSION_TYPE, 0);
                Num = InGameUtil.ParseInt(ref missionRoot, ConstantData.MAP_KEY_MISSION_NUM, 0);

                _attribute = AddressableManager.Instance.GetMissionAttribute(_type);

                UIManager.Instance.MissionUI.ImgMission.sprite = _attribute.SprMission;
            }

            public void ReduceMissionNum()
            {
                if(_isClear)
                {
                    return;
                }
                --Num;
                if(Num == 0)
                {
                    _isClear = true;
                    UIManager.Instance.MissionUI.TxtMissionNum.text = "Clear";
                }
            }
        }
    }
}
