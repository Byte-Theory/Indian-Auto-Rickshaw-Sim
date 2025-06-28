using UnityEngine;

public class FuelTank : MonoBehaviour
{
    [Header("Capacity")]
    [SerializeField] private float maxTankCapacity;
    [SerializeField] private float fuelLeftInTank;

    [Header("Burn Rate")] 
    [SerializeField] private float burnRate;
    [SerializeField] private float idleBurnRate;
    [SerializeField] private float runningBurnRate;
    [SerializeField] private float breakingBurnRate;
    
    // Data
    private bool isTankEmpty;
    
    // Refs
    private Player player;
    
    // Update is called once per frame
    void Update()
    {
        CheckIfTankEmpty();
        UpdateBurnRate();
        BurnFuel();
    }

    #region SetUp

    internal void SetUp(Player player)
    {
        this.player = player;
        
        fuelLeftInTank = maxTankCapacity * 0.75f;
        burnRate = idleBurnRate;
    }

    #endregion

    #region Fuel

    private void BurnFuel()
    {
        if (isTankEmpty)
        {
            return;
        }
        
        fuelLeftInTank -= Time.deltaTime * burnRate;

        if (fuelLeftInTank <= 0)
        {
            fuelLeftInTank = 0;
        }
    }

    private void CheckIfTankEmpty()
    {
        isTankEmpty = fuelLeftInTank <= 0;
    }
    
    #endregion
    
    #region Update Burn Rate

    private void UpdateBurnRate()
    {
        PlayerState playerState = player.GetPlayerState();
        Vector3 velocity = player.engine.GetVelocity();
        float speed = velocity.magnitude;

        if (playerState == PlayerState.Braking)
        {
            burnRate = breakingBurnRate;
        }
        else
        {
            if (speed < 0.1f)
            {
                burnRate = idleBurnRate;
            }
            else
            {
                burnRate = runningBurnRate;
            }
        }
    }

    #endregion
}
