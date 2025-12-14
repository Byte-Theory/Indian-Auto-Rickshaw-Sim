using System;
using UnityEngine;

public class PassengerOptimiser : MonoBehaviour
{
    private PassengerAnimManager passengerAnimManager;
    private PassengerVisuals passengerVisuals;
    
    #region SetUp

    public void SetUp(PassengerAnimManager passengerAnimManager, PassengerVisuals passengerVisuals)
    {
        this.passengerAnimManager = passengerAnimManager;
        this.passengerVisuals = passengerVisuals;
    }

    #endregion

    private void OnBecameVisible()
    {
        if (passengerAnimManager == null || passengerVisuals == null)
        {
            return;
        }
         
        passengerAnimManager.EnableAnimations();
        passengerVisuals.EnableVisuals();
    }

    private void OnBecameInvisible()
    {
        if (passengerAnimManager == null || passengerVisuals == null)
        {
            return;
        }
        
        passengerAnimManager.DisableAnimations();
        passengerVisuals.DisableVisuals();
    }
}
