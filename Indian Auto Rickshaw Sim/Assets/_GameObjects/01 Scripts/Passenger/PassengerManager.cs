using System;
using UnityEngine;

public class PassengerManager : MonoBehaviour
{
    [Header("Passenger")]
    public Passenger[] passengers;
    
    private PassengerPointManager passengerPointManager;

    private void Awake()
    {
        passengers = GetComponentsInChildren<Passenger>();
        passengerPointManager = GetComponentInChildren<PassengerPointManager>();
    }

    private void Start()
    {
        for (int idx = 0; idx < passengers.Length; idx++)
        {
            Passenger passenger = passengers[idx];
            passenger.SetUp(this);
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
}
