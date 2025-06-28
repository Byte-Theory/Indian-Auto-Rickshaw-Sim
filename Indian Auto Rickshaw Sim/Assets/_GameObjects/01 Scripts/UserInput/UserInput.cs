using System;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    [Header("Auto Input")]
    [SerializeField] private float turn;
    [SerializeField] private float acceleration;
    [SerializeField] private bool breaking;

    #region SingleTon

    public static UserInput Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    #endregion
    
    void Start()
    {
        ResetAutoInput();
    }

    void Update()
    {
        GetAutoInput();
    }

    #region Auto Input

    private void ResetAutoInput()
    {
        turn = 0;
        acceleration = 0;
    }
    
    private void GetAutoInput()
    {
        turn = Input.GetAxis("Horizontal");
        acceleration = Input.GetAxis("Vertical");
        breaking = Input.GetKey(KeyCode.Space);
    }

    #region Getters

    internal float GetTurn()
    {
        return turn;
    }

    internal float GetAcceleration()
    {
        return acceleration;
    }

    internal bool GetBreaking()
    {
        return breaking;
    }

    #endregion

    #endregion
}
