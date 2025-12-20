using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Passenger : MonoBehaviour
{
    [Header("Container")] 
    public GameObject ContainerGo;
    
    // States
    private PassengerStates curPassengerState = PassengerStates.Unknown;
    private PassengerStates prevPassengerState = PassengerStates.Unknown;
    private float curStateStartTime = 0.0f;
    private float curStateDur = 0.0f;
    private Vector3 StartStartPos;
    private Vector3 StartEndPos;
    private PassengerPoint targetPassengerPoint;
    private PassengerPoint targetPassengerPointForRide;
    private bool isAlsoLookingForARide = false;
    
    // Nav mesh
    private NavMeshAgent navMeshAgent;
    
    // Indicator
    private GameObject lookingForRideIndicator = null;
    
    // Callbacks
    private Action inRideCallback;
    
    // Ref
    private PassengerManager passengerManager;
    private PassengerAnimManager passengerAnimManager;
    private PassengerVisuals passengerVisuals;
    private PassengerOptimiser passengerOptimiser;
    private Player player;

    private void Update()
    {
        CheckAndCallAutoForRide();
        TickState();
    }

    #region SetUp

    public void SetUp(PassengerManager passengerManager, Player player)
    {
        this.passengerManager = passengerManager;
        this.player = player;

        passengerAnimManager = GetComponent<PassengerAnimManager>();
        passengerAnimManager.SetUp();

        passengerVisuals = GetComponent<PassengerVisuals>();
        passengerVisuals.SetUp();

        passengerOptimiser = GetComponentInChildren<PassengerOptimiser>();
        passengerOptimiser.SetUp(passengerAnimManager, passengerVisuals);
        
        PassengerPoint startingPoint = this.passengerManager.CalcRandPoint();
        transform.position = startingPoint.transform.position;
        
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true;
        
        ContainerGo.transform.localScale = Vector3.one;
        
        isAlsoLookingForARide = false;
        
        SetCurState(PassengerStates.Idle);
    }

    #endregion
    
    #region States

    private void SetCurState(PassengerStates newState, bool overrideCurState = false)
    {
        if (curPassengerState == newState && !overrideCurState)
        {
            return;
        }

        SetStateData(newState);

        prevPassengerState = curPassengerState;
        if (prevPassengerState == PassengerStates.Unknown || 
            prevPassengerState == PassengerStates.StoppedAtRoadCrossing || 
            prevPassengerState == PassengerStates.LookingForRide || 
            prevPassengerState == PassengerStates.CallingAutoForRide || 
            prevPassengerState == PassengerStates.GettingInRide || 
            prevPassengerState == PassengerStates.InRide || 
            prevPassengerState == PassengerStates.RideCompleted)
        {
            prevPassengerState = PassengerStates.Idle;
        }
        
        curPassengerState = newState;
        curStateStartTime = Time.time;
    }

    private void SetStateData(PassengerStates newState)
    {
        switch (newState)
        {
            case PassengerStates.Idle:
            {
                curStateDur = Random.Range(Constants.PassengerData.IdleDurationRange.x,
                    Constants.PassengerData.IdleDurationRange.y);

                navMeshAgent.isStopped = true;
                
                targetPassengerPointForRide = null;
                
                passengerAnimManager.PlayIdleAnim();
                break;
            }

            case PassengerStates.CalculatingNextState:
            {
                // Calculate next state in update
                break;
            }

            case PassengerStates.CalculatingNextMovePoint:
            {
                // Calculate point in update
                break;
            }

            case PassengerStates.Moving:
            {
                float moveSpeed = Constants.PassengerData.PassengerMoveSpeed +
                                  Random.Range(Constants.PassengerData.PassengerMoveSpeedOffset.x,
                                      Constants.PassengerData.PassengerMoveSpeedOffset.y);
                
                navMeshAgent.speed = moveSpeed;
                navMeshAgent.SetDestination(targetPassengerPoint.transform.position);
                
                navMeshAgent.isStopped = false;
                
                passengerAnimManager.PlayWalkAnim();
                break;
            }

            case PassengerStates.LookingForRide:
            {
                if (targetPassengerPointForRide == null)
                {
                    targetPassengerPointForRide = passengerManager.CalcNextPoint(transform.position);
                }

                navMeshAgent.isStopped = true;
                passengerAnimManager.PlayIdleAnim();
                break;
            }

            case PassengerStates.CallingAutoForRide:
            {
                if (targetPassengerPointForRide == null)
                {
                    targetPassengerPointForRide = passengerManager.CalcNextPoint(transform.position);
                }
                
                navMeshAgent.isStopped = true;
                passengerAnimManager.PlayWaveAnim();
                break;
            }

            case PassengerStates.GettingInRide:
            {
                navMeshAgent.enabled = false;
                passengerAnimManager.PlayIdleAnim();
                
                targetPassengerPoint = targetPassengerPointForRide;
                player.autoPassenger.SetDropOfPoint(targetPassengerPoint);
                
                curStateDur = Constants.PassengerData.GettingInRideDuration;
                
                RemoveLookingForRideIndicator();
                break;
            }

            case PassengerStates.InRide:
            {
                navMeshAgent.enabled = false;
                
                GameObject passengerContainerGo = player.autoPassenger.GetPassengerContainerGo();
                Transform passengerContainerT = passengerContainerGo.transform;
                transform.SetParent(passengerContainerT);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                
                ContainerGo.transform.localScale = Vector3.one * Constants.PassengerData.ScaleInAuto;
                
                passengerAnimManager.PlaySettingInAutoAnim();
                
                inRideCallback?.Invoke();
                break;
            }

            case PassengerStates.ExitingRide:
            {
                transform.SetParent(passengerManager.transform);
                
                StartStartPos = transform.position;
                StartEndPos = targetPassengerPoint.transform.position + 
                              new Vector3(Random.Range(-2.0f, 2.0f), 0.0f, Random.Range(-2.0f, 2.0f));
                
                passengerAnimManager.PlayIdleAnim();
                break;
            }

            case PassengerStates.RideCompleted:
            {
                targetPassengerPointForRide = null;
                
                navMeshAgent.enabled = true;
                break;
            }
        }
    }

    private void TickState()
    {
        if (curPassengerState == PassengerStates.CalculatingNextState)
        {
            bool wantToMove = Random.value < Constants.PassengerData.WantToMoveChance;
            SetCurState(wantToMove ? PassengerStates.CalculatingNextMovePoint : PassengerStates.Idle);
            return;
        }
        
        if (curPassengerState == PassengerStates.CalculatingNextMovePoint)
        {
            targetPassengerPoint = passengerManager.CalcNextPoint(transform.position);
            isAlsoLookingForARide = Random.value < Constants.PassengerData.RideRequirementChance;

            if (!isAlsoLookingForARide)
            {
                RemoveLookingForRideIndicator();
            }
            
            SetCurState(PassengerStates.Moving);
            return;
        }
        
        if (curPassengerState == PassengerStates.RideCompleted)
        {
            SetCurState(PassengerStates.Idle);
            return;
        }

        if (curPassengerState == PassengerStates.Idle)
        {
            float lapsedTime = Time.time - curStateStartTime;
            if (lapsedTime >= curStateDur)
            {
                SetCurState(PassengerStates.CalculatingNextState);
            }
        }
        else if (curPassengerState == PassengerStates.Moving)
        {
            bool isNavAgentReached = navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;
            if (isNavAgentReached)
            {
                SetCurState(PassengerStates.Idle);
            }
        }
        else if (curPassengerState == PassengerStates.CallingAutoForRide)
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.0001f)
            {
                return;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                10 * Time.deltaTime
            );
        }
        else if (curPassengerState == PassengerStates.GettingInRide)
        {
            float lapsedTime = Time.time - curStateStartTime;
            if (lapsedTime <= curStateDur)
            {
                float fac = lapsedTime / curStateDur;
                
                GameObject passengerContainerGo = player.autoPassenger.GetPassengerContainerGo();
                Transform passengerContainerT = passengerContainerGo.transform;
                Vector3 targetPos = passengerContainerT.position;
                
                transform.position = MathUtils.GetParabolaPoint(
                    transform.position,
                    targetPos,
                    Constants.PassengerData.JumpAnimHeight,
                    fac);
                
                ContainerGo.transform.localScale = Vector3.Lerp(
                    Vector3.one,
                    Vector3.one * Constants.PassengerData.ScaleInAuto,
                    fac);
            }
            else
            {
                SetCurState(PassengerStates.InRide);
            }
        }
        else if (curPassengerState == PassengerStates.ExitingRide)
        {
            float lapsedTime = Time.time - curStateStartTime;
            if (lapsedTime <= curStateDur)
            {
                float fac = lapsedTime / curStateDur;
                
                transform.position = MathUtils.GetParabolaPoint(
                    StartStartPos,
                    StartEndPos,
                    Constants.PassengerData.JumpAnimHeight,
                    fac);
                
                ContainerGo.transform.localScale = Vector3.Lerp(
                    Vector3.one * Constants.PassengerData.ScaleInAuto,
                    Vector3.one,
                    fac);
            }
            else
            {
                SetCurState(PassengerStates.RideCompleted);
            }
        }
    }

    #endregion

    #region Auto Detection

    public void SetAutoDetectedNearBy(bool isDetected, bool isAutoFull = false)
    {
        if (isAlsoLookingForARide)
        {
            //if (!isAutoFull)
            {
                if (isDetected)
                {
                    SetCurState(PassengerStates.LookingForRide);
                }
                else
                {
                    SetCurState(PassengerStates.Idle);
                }
            }

            ShowLookingForRideIndicator();
        }
        else
        {
            RemoveLookingForRideIndicator();
        }
    } 

    private void CheckAndCallAutoForRide()
    {
        bool isAutoEmpty = player.autoPassenger.GetCarryingPassenger() == null;
        
        if (isAutoEmpty && curPassengerState == PassengerStates.LookingForRide)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            
            bool callAutoForRide = distance < Constants.AutoData.CallingAutoForRideDetectionRadius;
            if (callAutoForRide)
            {
                SetCurState(PassengerStates.CallingAutoForRide);
            }
        }
    }

    public void MoveToAuto(Action inRideCallback)
    {
        this.inRideCallback = inRideCallback;
        SetCurState(PassengerStates.GettingInRide);
    }

    public void ExitTheRide()
    {
        SetCurState(PassengerStates.ExitingRide);
    }
    
    #endregion

    #region Looking for Ride Indicator

    private void ShowLookingForRideIndicator()
    {
        if (lookingForRideIndicator != null)
        {
            return;
        }
                
        lookingForRideIndicator = ObjectPooler.Instance.SpawnFromPool(1, transform.position, Quaternion.identity);
        lookingForRideIndicator.transform.SetParent(transform);
    }

    private void RemoveLookingForRideIndicator()
    {
        if (lookingForRideIndicator != null)
        {
            lookingForRideIndicator.SetActive(false);
            lookingForRideIndicator.transform.SetParent(ObjectPooler.Instance.transform);
            lookingForRideIndicator = null;
        }
    }

    #endregion
    
    #region Getters / Setters

    public PassengerStates GetCurPassengerState() => curPassengerState;
    
    public PassengerPoint GetTargetPassengerPoint() => targetPassengerPointForRide;

    #endregion
}
