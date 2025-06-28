using UnityEngine;

public class Player : MonoBehaviour
{
    // States
    private PlayerState playerState = PlayerState.Unknown;
    
    // Ref
    public Engine engine { get; private set; }
    public FuelTank fuelTank { get; private set; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        engine = GetComponent<Engine>();
        fuelTank = GetComponent<FuelTank>();
        
        engine.SetUp(this);
        fuelTank.SetUp(this);
        
        SetCurrentState(PlayerState.Running);
    }

    #region State

    internal PlayerState GetPlayerState()
    {
        return playerState;
    }
    
    internal void SetCurrentState(PlayerState newState)
    {
        if (newState == playerState)
        {
            return;
        }
        
        playerState = newState;
    }

    #endregion
}
