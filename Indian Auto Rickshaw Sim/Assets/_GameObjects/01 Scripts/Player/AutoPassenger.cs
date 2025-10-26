using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AutoPassenger : MonoBehaviour
{
    private Passenger carryingPassenger;
    private List<Passenger> selectedNearByPassengers = new List<Passenger>();
    
    // Ref
    private PassengerManager passengerManager;

    private void Update()
    {
        DetectNearByPassengers();
    }

    #region Set Up

    public void SetUp()
    {
        passengerManager = PassengerManager.Instance;
        
        carryingPassenger = null;
    }

    #endregion

    #region Passengers Detection

    private void DetectNearByPassengers()
    {
        // Removing the previously selected passengers if they are out of range
        List<int> indexToRemoveForNearByDetection = new List<int>();
        for (int i = 0; i < selectedNearByPassengers.Count; i++)
        {
            Passenger passenger = selectedNearByPassengers[i];
            float distance = Vector3.Distance(passenger.transform.position, transform.position);

            if (distance > Constants.AutoData.NearByPassengerDetectionRadius)
            {
                indexToRemoveForNearByDetection.Add(i);
            }
        }
        
        // Auto out of passenger detection range
        for (int i = 0; i < indexToRemoveForNearByDetection.Count; i++)
        {
            int index = indexToRemoveForNearByDetection[i];
            Passenger passenger = selectedNearByPassengers[index];
            passenger.SetAutoDetectedNearBy(false);
            selectedNearByPassengers.RemoveAt(index);
        }
        
        // Calc passengers for selected passengers
        Passenger[] allPassengers = passengerManager.GetAllPassengers();
        
        List<Passenger> nearByPassengers = new List<Passenger>();
        List<Passenger> newSelectedNearByPassengers = new List<Passenger>();
        int passengersRequired = Constants.AutoData.TotalNewByPassengerToSelect - selectedNearByPassengers.Count;

        for (int i = 0; i < allPassengers.Length; i++)
        {
            Passenger passenger = allPassengers[i];
            float distance = Vector3.Distance(passenger.transform.position, transform.position);

            if (distance < Constants.AutoData.NearByPassengerDetectionRadius)
            {
                nearByPassengers.Add(passenger);
            }
        }

        if (nearByPassengers.Count == 0)
        {
            return;
        }

        for (int i = 0; i < passengersRequired; i++)
        {
            if (nearByPassengers.Count == 0)
            {
                break;
            }
            
            int randIndex = Random.Range(0, nearByPassengers.Count);
            Passenger nearByPassenger = nearByPassengers[randIndex];

            if (selectedNearByPassengers.Contains(nearByPassenger))
            {
                continue;
            }
            
            newSelectedNearByPassengers.Add(nearByPassenger);
            selectedNearByPassengers.Add(nearByPassenger);
            
            nearByPassengers.RemoveAt(randIndex);
        }
        
        for (int i = 0; i < newSelectedNearByPassengers.Count; i++)
        {
            newSelectedNearByPassengers[i].SetAutoDetectedNearBy(true);
        }
    }

    #endregion
    
    #region Getters / Setters

    public Passenger GetCarryingPassenger() => carryingPassenger;

    #endregion

    private void OnDrawGizmos()
    {
        if (!Constants.DebugSettings.ShowGizmos)
        {
            return;
        }
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, Constants.AutoData.NearByPassengerDetectionRadius);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Constants.AutoData.CallingAutoForRideDetectionRadius);
    }
}
