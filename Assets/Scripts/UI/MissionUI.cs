using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JH
{
    namespace Match3Sample
    {
        public class MissionUI : MonoBehaviour
        {
            [SerializeField]
            private Image _imgMission;
            public Image ImgMission => _imgMission;


            [SerializeField]
            private Text _txtMissionNum;
            public Text TxtMissionNum => _txtMissionNum;

        }
    }
}
