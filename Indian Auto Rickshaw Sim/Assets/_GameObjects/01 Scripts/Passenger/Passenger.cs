using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Passenger : MonoBehaviour
{
    // States
    private PassengerStates curPassengerState = PassengerStates.Unknown;
    private float curStateStartTime = 0.0f;
    private float curStateDur = 0.0f;
    private PassengerPoint targetPassengerPoint;
    
    // Nav mesh
    private NavMeshAgent navMeshAgent;
    
    // Ref
    private PassengerManager passengerManager;
    private PassengerAnimManager passengerAnimManager;
    private PassengerVisuals passengerVisuals;

    private void Update()
    {
        TickState();
    }

    #region SetUp

    public void SetUp(PassengerManager passengerManager)
    {
        this.passengerManager = passengerManager;

        passengerAnimManager = GetComponent<PassengerAnimManager>();
        passengerAnimManager.SetUp();

        passengerVisuals = GetComponent<PassengerVisuals>();
        passengerVisuals.SetUp();
        
        PassengerPoint startingPoint = this.passengerManager.CalcRandPoint();
        transform.position = startingPoint.transform.position;
        
        navMeshAgent = GetComponent<NavMeshAgent>();
        
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
        }
    }

    private void TickState()
    {
        if (curPassengerState == PassengerStates.CalculatingNextState)
        {
            SetCurState(PassengerStates.CalculatingNextMovePoint);
            return;
        }
        
        if (curPassengerState == PassengerStates.CalculatingNextMovePoint)
        {
            targetPassengerPoint = passengerManager.CalcNextPoint(transform.position);
            SetCurState(PassengerStates.Moving);
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
    }

    #endregion
}
