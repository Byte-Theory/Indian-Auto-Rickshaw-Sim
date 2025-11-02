using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseMenuGo;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button homeButton;

    // Ref
    private GameplayTimeManager gameplayTimeManager;
    
    private void Awake()
    {
        pauseButton.onClick.AddListener(OnClickPauseButton);
        resumeButton.onClick.AddListener(OnClickResumeButton);
        homeButton.onClick.AddListener(OnClickHomeButton);
        
        pauseMenuGo.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameplayTimeManager = GameplayTimeManager.Instance;
    }

    #region Buttons

    private void OnClickPauseButton()
    {
        pauseMenuGo.SetActive(true);
        gameplayTimeManager.PauseGame();
    }

    private void OnClickResumeButton()
    {
        pauseMenuGo.SetActive(false);
        gameplayTimeManager.ResumeGame();
    }
    
    private void OnClickHomeButton()
    {
    }

    #endregion
}
