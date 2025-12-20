using UnityEngine;

public static class Constants
{
    public static class AutoData
    {
        // Passenger Detection
        public static int TotalNewByPassengerToSelect = 3;
        public static float NearByPassengerDetectionRadius = 65.0f;
        public static float CallingAutoForRideDetectionRadius = 25.0f;
        public static float PassengerPickUpDist = 5.0f;
        
        // Money
        public static float MoneyChargesPerUnitDistance = 5.0f;
        
        // Fuel
        public static float FuelRate = 100.0f;
    }
    
    public static class PassengerData
    {
        public static Vector2Int TotalPassengersInLevel = new Vector2Int(40, 70);
        public static float WantToMoveChance = 0.5f;
        public static float RideRequirementChance = 0.5f;
        
        // Speed
        public static float PassengerMoveSpeed = 2.0f;
        public static Vector2 PassengerMoveSpeedOffset = new Vector2(-0.25f, 0.25f);
        
        // Anim Dur
        public static Vector2 IdleDurationRange = new Vector2(0.5f, 3.0f);
        public static float GettingInRideDuration = 0.30f;
        public static float JumpAnimHeight = 0.60f;
        
        // Point Calculation
        public static float NextPointMinThreshold = 5.0f;
        
        // Scale
        public static float ScaleInAuto = 0.65f;
    }
    
    public static class PhoneConfigData
    {
        public static float PhonePuttingInPocketDur = 0.25f;
        public static float PhoneTakingOutPocketDur = 0.25f;
    }

    public static class DebugSettings
    {
        public static bool ShowGizmos = true;
    }
}
