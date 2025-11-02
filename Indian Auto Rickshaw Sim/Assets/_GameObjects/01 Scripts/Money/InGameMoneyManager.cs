using System;
using UnityEngine;

public class InGameMoneyManager : MonoBehaviour
{
    [Header("Money Data")] 
    [SerializeField] private float carryingMoney;

    // Ref
    private InGameMoneyUi inGameMoneyUi;
    
    #region Singleton

    public static InGameMoneyManager Instance;

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
        inGameMoneyUi = UiManager.Instance.GamePlayUi.InGameMoneyUi;
        
        carryingMoney = 0.0f;
        
        inGameMoneyUi.UpdateMoneyTxt(carryingMoney);
    }

    #region Calc Ride Charges

    public float CalcRideCharges(Passenger passenger)
    {
        PassengerPoint passengerPoint = passenger.GetTargetPassengerPoint();
        
        if(passengerPoint == null)
        {
            return -1.0f;
        }
        
        Vector3 curPos = passenger.transform.position;
        Vector3 targetPos = passengerPoint.transform.position;
        
        float distance = Vector3.Distance(curPos, targetPos);
        float rideCharges = distance * Constants.AutoData.MoneyChargesPerUnitDistance;
        
        return (int)rideCharges;
    }

    #endregion

    #region Payment

    public void TakePaymentFrom(Passenger passenger)
    {
        float payment = CalcRideCharges(passenger);

        if (payment >= 0)
        {
            carryingMoney += payment;
        }
        
        inGameMoneyUi.UpdateMoneyTxt(carryingMoney);
    }

    #endregion

    #region Money Operations

    public bool SpendMoney(float amt)
    {
        if (amt > carryingMoney)
        {
            return false;
        }
        
        carryingMoney -= amt;
        return true;
    }

    #endregion

    #region Getters / Setters

    public float CarryingMoney => carryingMoney;

    #endregion
}
