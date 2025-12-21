using System.Collections.Generic;
using UnityEngine;

public enum MissionType
{
    Unknown = -1,
    Passenger,
    EarnMoney,
    FillFuel
}

[CreateAssetMenu(fileName = "MissionData", menuName = "ThisGame/Data/MissionData")]
public class MissionData : ScriptableObject
{
    [System.Serializable]
    public class Data
    {
        public MissionType missionType;
        public string missionDescriptionPrefix;
        public string missionDescriptionSuffix;
        public Vector2Int missionTargetRange;
        public Vector2Int missionRewardRange;
        [HideInInspector] public float missionRequirement;
        [HideInInspector] public int missionReward;
    }
    
    public List<Data> allMissions;

    public Data CalcMissionData(MissionType missionType)
    {
        if (missionType == MissionType.Unknown)
        {
            return null;
        }
        
        for (int idx = 0; idx < allMissions.Count; idx++)
        {
            if (allMissions[idx].missionType == missionType)
            {
                Data newData = new Data();
                newData.missionType = allMissions[idx].missionType;
                newData.missionDescriptionPrefix = allMissions[idx].missionDescriptionPrefix;
                newData.missionDescriptionSuffix = allMissions[idx].missionDescriptionSuffix;
                newData.missionRequirement = Random.Range(allMissions[idx].missionTargetRange.x,
                    allMissions[idx].missionTargetRange.y);
                newData.missionReward = Random.Range(allMissions[idx].missionRewardRange.x,
                    allMissions[idx].missionRewardRange.y);

                return newData;
            }
        }
        
        return null;
    }
}
