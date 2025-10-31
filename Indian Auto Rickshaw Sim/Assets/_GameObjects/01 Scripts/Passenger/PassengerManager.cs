using UnityEngine;

public class PassengerManager : MonoBehaviour
{
    [Header("Player")] 
    public Player player;
    
    [Header("Passenger")]
    public Passenger[] passengers;
    
    // Passengers Tracker
    private int totalPassengersDroppedOff;
    
    // Ref
    private PassengerPointManager passengerPointManager;
    private PassengersUi passengersUi;

    #region SingleTon

    public static PassengerManager Instance;

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
        
        passengers = GetComponentsInChildren<Passenger>();
        passengerPointManager = GetComponentInChildren<PassengerPointManager>();
    }
    
    #endregion
    
    private void Start()
    {
        passengersUi = UiManager.Instance.GamePlayUi.PassengersUi;
        
        for (int idx = 0; idx < passengers.Length; idx++)
        {
            Passenger passenger = passengers[idx];
            passenger.SetUp(this, player);
        }

        totalPassengersDroppedOff = 0;
        passengersUi.UpdatePassengerDropCt(totalPassengersDroppedOff);
    }

    #region Passenger Point Manager

    public PassengerPoint CalcRandPoint()
    {
        return passengerPointManager.CalcRandPoint();
    }

    public PassengerPoint CalcNextPoint(Vector3 passengerPosition)
    {
        return passengerPointManager.CalcNextPoint(passengerPosition);
    }

    #endregion

    #region DropOff

    public void PassengerDropOff(Passenger passenger)
    {
        totalPassengersDroppedOff++;
        passengersUi.UpdatePassengerDropCt(totalPassengersDroppedOff);
    }

    #endregion
    
    #region Getters

    public Passenger[] GetAllPassengers() => passengers;

    #endregion
}
