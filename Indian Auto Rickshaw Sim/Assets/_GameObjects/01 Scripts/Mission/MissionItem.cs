using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionItem : MonoBehaviour
{
    [Header("Ui")] 
    [SerializeField] private TMP_Text missionDesc;
    [SerializeField] private TMP_Text missionReward;
    [SerializeField] private Image missionButtonImg;
    [SerializeField] private Button claimButton;

    [Header("Sprites")]
    [SerializeField] private Sprite buttonActiveSprite;
    [SerializeField] private Sprite buttonInactiveSprite;
    
    // Mission data
    private MissionData.Data missionData;

    #region SetUp

    public void SetUp(MissionData.Data data)
    {
        this.missionData = data;

        missionDesc.text = data.missionDescriptionPrefix + " " + 
                           data.missionRequirement + " " +
                           data.missionDescriptionSuffix;
        
        missionReward.text = data.missionReward.ToString();
        missionButtonImg.sprite = missionData.missionRequirement > 0 ? buttonInactiveSprite :  buttonActiveSprite;
        
        claimButton.onClick.RemoveAllListeners();
        claimButton.onClick.AddListener(OnClickClaimButton);
    }
    
    public void UpdateMissionData(MissionData.Data data)
    {
        if (missionData.missionType != data.missionType)
        {
            return;
        }
        
        this.missionData = data;

        missionDesc.text = data.missionDescriptionPrefix + " " + 
                           data.missionRequirement + " " +
                           data.missionDescriptionSuffix;
        
        missionReward.text = data.missionReward.ToString();
        missionButtonImg.sprite = missionData.missionRequirement > 0 ? buttonInactiveSprite :  buttonActiveSprite;
    }

    #endregion

    #region Buttion

    private void OnClickClaimButton()
    {
        bool isClaimed = MissionManager.Instance.GetMissionReward(missionData.missionType);

        if (isClaimed)
        {
            MissionManager.Instance.GetNewMissionReward(missionData.missionType);
        }
        
        AudioManager.Instance.PlayAudio(AudioClipType.ButtonClick);
    }

    #endregion
}
