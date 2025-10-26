using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PassengerPointManager : MonoBehaviour
{
    private PassengerPoint[] passengerPoints;

    private void Awake()
    {
        passengerPoints = GetComponentsInChildren<PassengerPoint>();
    }

    public PassengerPoint CalcRandPoint()
    {
        if (passengerPoints == null || passengerPoints.Length == 0)
        {
            Debug.LogError("PassengerPointManager is null or empty");
            return null;
        }

        PassengerPoint passengerPoint = passengerPoints[0];
        
        int randIndex = Random.Range(0, passengerPoints.Length);
        passengerPoint = passengerPoints[randIndex];
        
        return passengerPoint;
    }
    
    public PassengerPoint CalcNextPoint(Vector3 curPassengerPos)
    {
        if (passengerPoints == null || passengerPoints.Length == 0)
        {
            Debug.LogError("PassengerPointManager is null or empty");
            return null;
        }
        
        PassengerPoint passengerPoint = passengerPoints[0];
        
        List<PassengerPoint> validPoints = new List<PassengerPoint>();
        for (int i = 0; i < passengerPoints.Length; i++)
        {
            PassengerPoint point = passengerPoints[i];
            float dist = Vector3.Distance(point.transform.position, curPassengerPos);

            if (dist > Constants.PassengerData.NextPointMinThreshold)
            {
                validPoints.Add(point);
            }
        }

        if (validPoints.Count > 0)
        {
            int randIndex = Random.Range(0, validPoints.Count);
            passengerPoint = validPoints[randIndex];
        }
        
        return passengerPoint;
    }
}
