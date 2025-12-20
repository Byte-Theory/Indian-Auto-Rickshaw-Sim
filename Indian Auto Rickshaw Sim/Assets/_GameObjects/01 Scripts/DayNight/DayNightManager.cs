using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DayPart
{
    public string partName;
    
    [Space]
    
    public float startTime;
    public float endTime;

    [Header("Light Data")] 
    public Color mainLightCol;
    public float mainLightIntensity;
    public Color ambLightCol;
    public float ambLightIntensity;
    public Color fogColor;
    public float fogIntensity;
}

public class DayNightManager : MonoBehaviour
{
    private static readonly int SkyMatBlendId = Shader.PropertyToID("_Blend");

    [Header("Direction Light")]
    [SerializeField] private Light mainDirLight;
    [SerializeField] private Light ambDirLight;
    private Vector3 mainLightRotEuler;

    [Header("Day Night Data")]
    [SerializeField] private float timePassSpeed;
    [SerializeField] private float hrsInDay = 24.0f;
    [SerializeField] private float lightChangeSpeed = 2.0f;
    [SerializeField] private List<DayPart> dayParts;
    
    [Space]
    
    [SerializeField] private DayPart currentDayPart;
    [SerializeField] private float timeOfDay;
    
    [Header("Sky box")]
    [SerializeField] private Material skyboxBlendMat;
    private float skyBlend;
    
    // Fog
    private Color fogColor;
    private float fogIntensity;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetUpLogic();
        SetUpSkyBox();
    }

    // Update is called once per frame
    void Update()
    {
        TickTimeOfDay();
        UpdateDayParts();
        RotateMainLight();
        UpdateLightColorAndIntensity();
        UpdateSkyBox();
    }

    #region DayNight Logic

    private void SetUpLogic()
    {
        timeOfDay = 5.0f;
        mainLightRotEuler = mainDirLight.transform.eulerAngles;
    }

    private void TickTimeOfDay()
    {
        timeOfDay += Time.deltaTime * timePassSpeed;

        if (timeOfDay >= hrsInDay)
        {
            timeOfDay = 0.0f;
        }
    }

    private void UpdateDayParts()
    {
        for (int idx = 0; idx < dayParts.Count; idx++)
        {
            DayPart dayPart = dayParts[idx];
            float startTime = dayPart.startTime;
            float endTime = dayPart.endTime;

            if (timeOfDay > startTime && timeOfDay <= endTime)
            {
                currentDayPart = dayPart;
            }
        }
    }

    #endregion

    #region Direction Lights

    private void RotateMainLight()
    {
        float delta = timeOfDay / hrsInDay;
        float xRot = Mathf.PingPong(delta, 1.0f) * 180f;
        
        mainDirLight.transform.rotation = Quaternion.Euler(
            xRot,
            mainLightRotEuler.y,
            mainLightRotEuler.z);
    }

    private void UpdateLightColorAndIntensity()
    {
        mainDirLight.color = Color.Lerp(mainDirLight.color, currentDayPart.mainLightCol, Time.deltaTime * lightChangeSpeed);
        ambDirLight.color = Color.Lerp(ambDirLight.color, currentDayPart.ambLightCol, Time.deltaTime * lightChangeSpeed);
        
        mainDirLight.intensity = Mathf.Lerp(mainDirLight.intensity, currentDayPart.mainLightIntensity, Time.deltaTime * lightChangeSpeed);
        ambDirLight.intensity = Mathf.Lerp(ambDirLight.intensity, currentDayPart.ambLightIntensity, Time.deltaTime * lightChangeSpeed);
    }

    #endregion

    #region Sky box

    private void SetUpSkyBox()
    {
        DayPart morningDayPart = dayParts[0];
        DayPart noonDayPart = dayParts[1];
        DayPart eveningDayPart = dayParts[2];
        DayPart nightDayPart = dayParts[3];

        if (timeOfDay > morningDayPart.startTime && timeOfDay <= morningDayPart.endTime)
        {
            float morningDur = morningDayPart.endTime - morningDayPart.startTime;
            float timeSinceMorningStarted = timeOfDay - morningDayPart.startTime;
            skyBlend = timeSinceMorningStarted / morningDur;
            skyboxBlendMat.SetFloat(SkyMatBlendId, 1.0f - skyBlend);
            
            fogColor = morningDayPart.fogColor;
            fogIntensity = morningDayPart.fogIntensity;
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogIntensity / 100000.0f; // 0.00375
        }
        else if (timeOfDay > noonDayPart.startTime && timeOfDay <= noonDayPart.endTime)
        {
            skyBlend = 0.0f;
            skyboxBlendMat.SetFloat(SkyMatBlendId, skyBlend);
            
            fogColor = noonDayPart.fogColor;
            fogIntensity = noonDayPart.fogIntensity;
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogIntensity / 100000.0f; // 0.00375
        }
        else if (timeOfDay > eveningDayPart.startTime && timeOfDay <= eveningDayPart.endTime)
        {
            float eveningDur = eveningDayPart.endTime - eveningDayPart.startTime;
            float timeSinceEveningStarted = timeOfDay - eveningDayPart.startTime;
            skyBlend = timeSinceEveningStarted / eveningDur;
            skyboxBlendMat.SetFloat(SkyMatBlendId, skyBlend);
            
            fogColor = eveningDayPart.fogColor;
            fogIntensity = eveningDayPart.fogIntensity;
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogIntensity / 100000.0f; // 0.00375
        }
        else if (timeOfDay > nightDayPart.startTime && timeOfDay <= nightDayPart.endTime)
        {
            skyBlend = 1.0f;
            skyboxBlendMat.SetFloat(SkyMatBlendId, skyBlend);
            
            fogColor = nightDayPart.fogColor;
            fogIntensity = nightDayPart.fogIntensity;
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogIntensity / 100000.0f; // 0.00375
        }
    }
    
    private void UpdateSkyBox()
    {
        DayPart morningDayPart = dayParts[0];
        DayPart noonDayPart = dayParts[1];
        DayPart eveningDayPart = dayParts[2];
        DayPart nightDayPart = dayParts[3];

        if (timeOfDay > morningDayPart.startTime && timeOfDay <= morningDayPart.endTime)
        {
            float morningDur = morningDayPart.endTime - morningDayPart.startTime;
            float timeSinceMorningStarted = timeOfDay - morningDayPart.startTime;
            skyBlend = timeSinceMorningStarted / morningDur;
            skyboxBlendMat.SetFloat(SkyMatBlendId, 1.0f - skyBlend);
            
            fogColor = Color.Lerp(fogColor, morningDayPart.fogColor, Time.deltaTime * lightChangeSpeed);
            fogIntensity = Mathf.Lerp(fogIntensity, morningDayPart.fogIntensity, Time.deltaTime * lightChangeSpeed);
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogIntensity / 100000.0f; // 0.00375
        }
        else if (timeOfDay > noonDayPart.startTime && timeOfDay <= noonDayPart.endTime)
        {
            skyBlend = 0.0f;
            skyboxBlendMat.SetFloat(SkyMatBlendId, skyBlend);
            
            fogColor = Color.Lerp(fogColor, noonDayPart.fogColor, Time.deltaTime * lightChangeSpeed);
            fogIntensity = Mathf.Lerp(fogIntensity, noonDayPart.fogIntensity, Time.deltaTime * lightChangeSpeed);
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogIntensity / 100000.0f; // 0.00375
        }
        else if (timeOfDay > eveningDayPart.startTime && timeOfDay <= eveningDayPart.endTime)
        {
            float eveningDur = eveningDayPart.endTime - eveningDayPart.startTime;
            float timeSinceEveningStarted = timeOfDay - eveningDayPart.startTime;
            skyBlend = timeSinceEveningStarted / eveningDur;
            skyboxBlendMat.SetFloat(SkyMatBlendId, skyBlend);
            
            fogColor = Color.Lerp(fogColor, eveningDayPart.fogColor, Time.deltaTime * lightChangeSpeed);
            fogIntensity = Mathf.Lerp(fogIntensity, eveningDayPart.fogIntensity, Time.deltaTime * lightChangeSpeed);
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogIntensity / 100000.0f; // 0.00375
        }
        else if (timeOfDay > nightDayPart.startTime && timeOfDay <= nightDayPart.endTime)
        {
            skyBlend = 1.0f;
            skyboxBlendMat.SetFloat(SkyMatBlendId, skyBlend);
            
            fogColor = Color.Lerp(fogColor, nightDayPart.fogColor, Time.deltaTime * lightChangeSpeed);
            fogIntensity = Mathf.Lerp(fogIntensity, nightDayPart.fogIntensity, Time.deltaTime * lightChangeSpeed);
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogIntensity / 100000.0f; // 0.00375
        }
    }

    #endregion
}
