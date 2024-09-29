using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {
        public class MissionManager : SingletonManager<MissionManager>
        {

            [SerializeField]
            private Mission[] _missions;

            public void LoadMissions(LitJson.JsonData missionsRoot)
            {
                _missions = new Mission[missionsRoot.Count];
                for(int i = 0; i < missionsRoot.Count; ++i)
                {
                    _missions[i] = new Mission();
                    _missions[i].Init(missionsRoot[i]);
                }
            }

        }
    }
}
