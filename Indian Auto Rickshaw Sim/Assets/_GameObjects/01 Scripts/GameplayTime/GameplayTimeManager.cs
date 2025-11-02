using System;
using UnityEngine;

public class GameplayTimeManager : MonoBehaviour
{
    // Time Scale Data
    private float timeScale;
    private float timeScaleChangeSpeed = 2.0f;
    
    #region Singleton

    public static GameplayTimeManager Instance;

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

    private void Start()
    {
        SetUp();
    }

    #region SetUp

    public void SetUp()
    {
        ResumeGame();
    }

    private void SetTimeScale()
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = 0.02f * timeScale;
    }

    #endregion

    #region Pause And Resume

    public void PauseGame()
    {
        timeScale = 0;
        SetTimeScale();
    }

    public void ResumeGame()
    {
        timeScale = 1;
        SetTimeScale();
    }

    #endregion
}
