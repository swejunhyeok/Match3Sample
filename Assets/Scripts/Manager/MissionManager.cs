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

            public void ReduceMission(MissionType type)
            {
                for(int i = 0; i < _missions.Length; ++i)
                {
                    if (_missions[i].Type == type && !_missions[i].IsClear)
                    {
                        _missions[i].ReduceMissionNum();
                        if(IsAllMissionClear())
                        {
                            GameManager.Instance.AddGameState(GameManager.GameState.Done_GameEnd);
                        }
                    }
                }
            }

            private bool IsAllMissionClear()
            {
                bool isAllClear = true;
                for(int i = 0; i < _missions.Length && isAllClear; ++i)
                {
                    isAllClear = _missions[i].IsClear;
                }
                return isAllClear;
            }
        }
    }
}
