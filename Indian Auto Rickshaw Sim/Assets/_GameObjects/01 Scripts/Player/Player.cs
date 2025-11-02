using UnityEngine;

public class Player : MonoBehaviour
{
    // States
    private PlayerState playerState = PlayerState.Unknown;
    
    // Ref
    public Engine engine { get; private set; }
    public FuelTank fuelTank { get; private set; }
    public AutoPassenger autoPassenger { get; private set; }
    
    #region Singleton

    public static Player Instance;

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
        engine = GetComponent<Engine>();
        fuelTank = GetComponent<FuelTank>();
        autoPassenger = GetComponent<AutoPassenger>();
        
        engine.SetUp(this);
        fuelTank.SetUp(this);
        autoPassenger.SetUp();
        
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
