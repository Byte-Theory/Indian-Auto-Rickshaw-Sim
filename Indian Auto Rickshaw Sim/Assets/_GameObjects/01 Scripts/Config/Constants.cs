using UnityEngine;

public static class Constants
{
    public static class AutoData
    {
        // Passenger Detection
        public static int TotalNewByPassengerToSelect = 3;
        public static float NearByPassengerDetectionRadius = 65.0f;
        public static float CallingAutoForRideDetectionRadius = 25.0f;
    }
    
    public static class PassengerData
    {
        // Speed
        public static float PassengerMoveSpeed = 2.0f;
        public static Vector2 PassengerMoveSpeedOffset = new Vector2(-0.25f, 0.25f);
        
        // Anim Dur
        public static Vector2 IdleDurationRange = new Vector2(0.25f, 2.5f);
        
        // Point Calculation
        public static float NextPointMinThreshold = 5.0f;
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
