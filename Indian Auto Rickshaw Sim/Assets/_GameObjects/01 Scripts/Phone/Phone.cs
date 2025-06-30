using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{
    [Header("Phone Elements")]
    [SerializeField] private GameObject container;
    
    [Header("Phone Poses")]
    [SerializeField] private Vector3 phoneInPocketPos;
    [SerializeField] private Vector3 phoneUsingPos;

    [Header("App Data")] 
    [SerializeField] private List<PhoneAppData> allAppData;
    
    // Phone State
    private PhoneState phoneState = PhoneState.Unknown;
    private float stateTimeElapsed = 0.0f;
    private float stateDuration = 0.0f;
    private Vector3 stateStartPos;
    private Vector3 stateEndPos;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        CheckAndTakeOutOrPutInPocket();
        UpdateStateTimer();
    }

    #region SetUp

    private void SetUp()
    {
        SetPhoneState(PhoneState.InPocket);
    }

    #endregion

    #region State

    private void SetPhoneState(PhoneState newState)
    {
        if (newState == phoneState)
        {
            return;
        }

        if (newState == PhoneState.PuttingInPocket)
        {
            stateStartPos = phoneUsingPos;
            stateEndPos = phoneInPocketPos;

            container.SetActive(true);
            
            stateDuration = Constants.PhoneConfigData.PhonePuttingInPocketDur;
        }
        else if (newState == PhoneState.InPocket)
        {
            container.transform.localPosition = phoneInPocketPos;
            
            container.SetActive(false);
        }
        else if (newState == PhoneState.TakingOutPocket)
        {
            stateStartPos = phoneInPocketPos;
            stateEndPos = phoneUsingPos;

            container.SetActive(true);
            
            stateDuration = Constants.PhoneConfigData.PhoneTakingOutPocketDur;
        }
        else if (newState == PhoneState.UsingPhone)
        {
            container.transform.localPosition = phoneUsingPos;
            
            container.SetActive(true);
        }
        
        phoneState = newState;
        stateTimeElapsed = 0;
    }

    private void UpdateStateTimer()
    {
        if (phoneState == PhoneState.PuttingInPocket)
        {
            stateTimeElapsed += Time.deltaTime;

            if (stateTimeElapsed < stateDuration)
            {
                float fac = stateTimeElapsed / stateDuration;
                container.transform.localPosition = Vector3.Lerp(stateStartPos, stateEndPos, fac);
            }
            else
            {
                SetPhoneState(PhoneState.InPocket);
            }
        }
        else if (phoneState == PhoneState.TakingOutPocket)
        {
            stateTimeElapsed += Time.deltaTime;

            if (stateTimeElapsed < stateDuration)
            {
                float fac = stateTimeElapsed / stateDuration;
                container.transform.localPosition = Vector3.Lerp(stateStartPos, stateEndPos, fac);
            }
            else
            {
                SetPhoneState(PhoneState.UsingPhone);
            }
        }
    }
    
    #endregion

    #region Phone Usings

    private void CheckAndTakeOutOrPutInPocket()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (phoneState == PhoneState.PuttingInPocket || phoneState == PhoneState.InPocket)
            {
                SetPhoneState(PhoneState.TakingOutPocket);
            }
            else if (phoneState == PhoneState.TakingOutPocket || phoneState == PhoneState.UsingPhone)
            {
                SetPhoneState(PhoneState.PuttingInPocket);
            }
        }
    }

    #endregion
}
