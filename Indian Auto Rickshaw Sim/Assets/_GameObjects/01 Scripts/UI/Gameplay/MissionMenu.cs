using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionMenu : MonoBehaviour
{
    [Header("Ui")]
    [SerializeField] private GameObject missionPanel;

    [Header("Mission Item")]
    [SerializeField] private GameObject missionItemParent;
    [SerializeField] private GameObject missionItemPrefab;
    [SerializeField] private float startY;
    [SerializeField] private float gapY;
    private List<MissionItem> missionItems = new List<MissionItem>();
    
    [Header("Buttons")]
    [SerializeField] private Button openMenuButton;
    [SerializeField] private Button closeMenuButton;

    private void Start()
    {
        SetUp();
    }

    #region SetUp

    public void SetUp()
    {
        missionPanel.SetActive(false);
        
        openMenuButton.onClick.AddListener(OnClickOpenMenuButton);
        closeMenuButton.onClick.AddListener(OnClickCloseMenuButton);
    }

    public void SpawnMissionItems(List<MissionData.Data> missionData)
    {
        missionItems = new List<MissionItem>();
        
        for (int idx = 0; idx < missionData.Count; idx++)
        {
            MissionData.Data data = missionData[idx];
            
            GameObject missionItemGo = Instantiate(missionItemPrefab, missionItemParent.transform);
            RectTransform missionItemRectTransform = missionItemGo.GetComponent<RectTransform>();
            missionItemRectTransform.anchoredPosition = new Vector2(0.0f, startY + gapY * idx);
            
            MissionItem missionItem =  missionItemGo.GetComponent<MissionItem>();
            missionItem.SetUp(data);
            
            missionItems.Add(missionItem);
        }
    }
    
    #endregion
    
    public void UpdateMissionItem(MissionData.Data missionData)
    {
        for (int idx = 0; idx < missionItems.Count; idx++)
        {
            missionItems[idx].UpdateMissionData(missionData);   
        }
    }

    #region Button

    private void OnClickOpenMenuButton()
    {
        missionPanel.SetActive(true);   
        
        AudioManager.Instance.PlayAudio(AudioClipType.ButtonClick);
        
        GameplayTimeManager.Instance.PauseGame();
    }

    private void OnClickCloseMenuButton()
    {
        missionPanel.SetActive(false);

        AudioManager.Instance.PlayAudio(AudioClipType.ButtonClick);
            
        GameplayTimeManager.Instance.ResumeGame();
    }

    #endregion
}
