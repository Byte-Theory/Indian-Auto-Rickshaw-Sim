using System;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [Header("Menu Refs")]
    [SerializeField] private GamePlayUi gamePlayUi;

    #region Singleton

    public static UiManager Instance;

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
    
    #region Getters / Setters

    public GamePlayUi GamePlayUi => gamePlayUi;

    #endregion
}
