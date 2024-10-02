using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {

        public enum MissionType
        {
            None,
            Gimmick_1
        }

        [CreateAssetMenu(fileName = "Mission Attribute", menuName = "JH/Match3Sample/Mission Attribute")]
        public class MissionAttribute : ScriptableObject
        {
            [SerializeField]
            private MissionType _type;
            public MissionType Type
            {
                get => _type;
            }

            [SerializeField]
            private Sprite _sprMission;
            public Sprite SprMission
            {
                get => _sprMission;
            }
        }
    }
}
