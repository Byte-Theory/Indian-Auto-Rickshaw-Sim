using System.Collections.Generic;
using UnityEngine;

public class PassengerManager : MonoBehaviour
{
    [Header("Player")] 
    public Player player;
    
    [Header("Passenger")]
    public GameObject passengerPrefab;
    public List<Passenger> passengers;
    
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
        
        passengerPointManager = GetComponentInChildren<PassengerPointManager>();
    }
    
    #endregion
    
    private void Start()
    {
        passengersUi = UiManager.Instance.GamePlayUi.PassengersUi;

        SpawnAllPassengers();
        
        totalPassengersDroppedOff = 0;
        passengersUi.UpdatePassengerDropCt(totalPassengersDroppedOff);
    }

    private void SpawnAllPassengers()
    {
        int totalPassengers = Random.Range(Constants.PassengerData.TotalPassengersInLevel.x,
            Constants.PassengerData.TotalPassengersInLevel.y);

        passengers = new List<Passenger>();
        
        for (int idx = 0; idx < totalPassengers; idx++)
        {
            GameObject go = Instantiate(passengerPrefab, transform);
            
            Passenger passenger = go.GetComponent<Passenger>();
            passenger.SetUp(this, player);
            
            passengers.Add(passenger);
        }
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

    public List<Passenger> GetAllPassengers() => passengers;

    #endregion
}
