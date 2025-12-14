using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AutoPassenger : MonoBehaviour
{
    [Header("Container And Arrow")]
    public GameObject passengerContainer;
    public GameObject directionArrow;
    
    private Passenger carryingPassenger = null;
    private List<Passenger> selectedNearByPassengers = new List<Passenger>();
    
    private PassengerPoint dropOfPoint = null;
    
    // Ref
    private PassengerManager passengerManager;
    private InGameMoneyManager inGameMoneyManager;

    private void Update()
    {
        DetectNearByPassengers();
        TryPickUpPassenger();
        CheckIfDropPointReached();
        UpdateDirectionArrowDirection();
    }

    #region Set Up

    public void SetUp()
    {
        passengerManager = PassengerManager.Instance;
        inGameMoneyManager = InGameMoneyManager.Instance;
        
        carryingPassenger = null;
        
        ShowDirectionArrow(false);
    }

    #endregion

    #region Passengers Detection / PickUp

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
        List<Passenger> allPassengers = passengerManager.GetAllPassengers();
        
        List<Passenger> nearByPassengers = new List<Passenger>();
        List<Passenger> newSelectedNearByPassengers = new List<Passenger>();
        int passengersRequired = Constants.AutoData.TotalNewByPassengerToSelect - selectedNearByPassengers.Count;

        for (int i = 0; i < allPassengers.Count; i++)
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

    private void TryPickUpPassenger()
    {
        if (carryingPassenger != null)
        {
            return;
        }
        
        if (selectedNearByPassengers.Count == 0)
        {
            return;
        }

        Passenger passengetToPick = null;
        
        for (int i = 0; i < selectedNearByPassengers.Count; i++)
        {
            Passenger passenger = selectedNearByPassengers[i];
            float distance = Vector3.Distance(passenger.transform.position, transform.position);

            if (distance < Constants.AutoData.PassengerPickUpDist)
            {
                passengetToPick = passenger;
                break;
            }
        }

        if (passengetToPick == null)
        {
            return;
        }

        PassengerStates passengerState = passengetToPick.GetCurPassengerState();
        if (passengerState == PassengerStates.CallingAutoForRide)
        {
            carryingPassenger = passengetToPick;
            passengetToPick.MoveToAuto();
            
            ShowDirectionArrow(true);
        }
    }

    public void SetDropOfPoint(PassengerPoint dropPoint)
    {
        dropOfPoint = dropPoint;
    }

    private void CheckIfDropPointReached()
    {
        if(carryingPassenger == null || dropOfPoint == null)
        {
            return;
        }
        
        float distance = Vector3.Distance(dropOfPoint.transform.position, transform.position);

        if (distance < Constants.AutoData.PassengerPickUpDist)
        {
            inGameMoneyManager.TakePaymentFrom(carryingPassenger);
            passengerManager.PassengerDropOff(carryingPassenger);
            
            carryingPassenger.ExitTheRide();
            carryingPassenger = null;
            dropOfPoint = null;

            ShowDirectionArrow(false);
        }
    }

    #endregion

    #region Direction Arrow

    private void ShowDirectionArrow(bool show)
    {
        directionArrow.SetActive(show);
    }

    private void UpdateDirectionArrowDirection()
    {
        if (carryingPassenger == null || dropOfPoint == null)
        {
            return;
        }
        
        Vector3 direction = dropOfPoint.transform.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        
        directionArrow.transform.rotation = Quaternion.Lerp(
            directionArrow.transform.rotation,
            targetRotation, 
            Time.deltaTime * 5.0f);
    }

    #endregion
    
    #region Getters / Setters

    public Passenger GetCarryingPassenger() => carryingPassenger;
    public GameObject GetPassengerContainerGo() => passengerContainer;

    #endregion

    #region Gizmos
    
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
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, Constants.AutoData.PassengerPickUpDist);
    }
    
    #endregion
}
