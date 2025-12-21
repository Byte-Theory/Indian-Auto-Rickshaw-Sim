using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private MissionData missionData;
    [SerializeField] private bool[] missionCompletionCheck;
    
    // Mission
    private List<MissionData.Data> allMissionData;
    
    // ref
    private MissionMenu missionMenu;
    
    #region Singleton

    public static MissionManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    #endregion
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {  
        SetUp();
    }

    #region SetUp

    public void SetUp()
    {
        allMissionData = new List<MissionData.Data>();

        for (int idx = 0; idx < System.Enum.GetValues(typeof(MissionType)).Length; idx++)
        {
            MissionData.Data data = missionData.CalcMissionData((MissionType)idx);
            
            if (data == null)
            {
                continue;
            }
            
            allMissionData.Add(data);
        }
        
        missionCompletionCheck = new bool[allMissionData.Count];

        missionMenu = UiManager.Instance.GamePlayUi.MissionMenu;
        missionMenu.SpawnMissionItems(allMissionData);
    }

    #endregion

    #region Mission Progress and Reward

    public void OnUpdateMissionProgress(MissionType missionType, int value)
    {
        for (int idx = 0; idx < allMissionData.Count; idx++)
        {
            MissionData.Data data = allMissionData[idx];
            if (data.missionType == missionType)
            {
                if (data.missionRequirement <= 0 || missionCompletionCheck[idx])
                {
                    return;
                }
                
                data.missionRequirement -= value;
                if (data.missionRequirement <= 0)
                {
                    data.missionRequirement = 0;
                    missionCompletionCheck[idx] = true;
                }
                
                missionMenu.UpdateMissionItem(data);
            }
        }
    }

    public bool GetMissionReward(MissionType missionType)
    {
        for (int idx = 0; idx < allMissionData.Count; idx++)
        {
            MissionData.Data data = allMissionData[idx];
            if (data.missionType == missionType)
            {
                if (data.missionRequirement <= 0 || missionCompletionCheck[idx])
                {
                    InGameMoneyManager.Instance.AddMoney(data.missionReward);
                    
                    AudioManager.Instance.PlayAudio(AudioClipType.MissionClaim);

                    return true;
                }
            }
        }
        
        return false;
    }

    public void GetNewMissionReward(MissionType missionType)
    {
        for (int idx = 0; idx < allMissionData.Count; idx++)
        {
            MissionData.Data data = allMissionData[idx];
            if (data.missionType == missionType)
            {
                data = missionData.CalcMissionData((MissionType)idx);
                missionMenu.UpdateMissionItem(data);
            }
        }
    }

    #endregion
}
