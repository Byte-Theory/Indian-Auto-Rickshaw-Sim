using System;
using UnityEngine;


public class Engine : MonoBehaviour
{
    [Header("Driving Data")]
    [SerializeField] private float maxMotorForce;
    [SerializeField] private float maxSteerAngle;
    
    [Header("Wheel Transforms")]
    [SerializeField] private Transform frontWheelT;
    [SerializeField] private Transform backLeftWheelT;
    [SerializeField] private Transform backRightWheelT;
    
    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider frontWheelCollider;
    [SerializeField] private WheelCollider frontHiddenLeftWheelCollider;
    [SerializeField] private WheelCollider frontHiddenRightWheelCollider;
    [SerializeField] private WheelCollider backLeftWheelCollider;
    [SerializeField] private WheelCollider backRightWheelCollider;
    
    // Data Variables
    private Rigidbody rb;
    private bool isBreaking;
    
    private Player player;
    private UserInput userInput;
    
    void Update()
    {
        UpdateWheelVisuals(frontWheelCollider, frontWheelT);
        UpdateWheelVisuals(backLeftWheelCollider, backLeftWheelT);
        UpdateWheelVisuals(backRightWheelCollider, backRightWheelT);
    }

    private void FixedUpdate()
    {
        GetBreakingInput();
        
        Drive();
        Turn();
    }

    #region SetUp

    internal void SetUp(Player player)
    {
        this.player = player;
        
        userInput = UserInput.Instance;
        
        rb = GetComponent<Rigidbody>();
    }

    #endregion

    #region Driving / Turning

    private void Drive()
    {
        if (isBreaking)
        {
            backLeftWheelCollider.brakeTorque = maxMotorForce * 1.5f;
            backRightWheelCollider.brakeTorque = maxMotorForce * 1.5f;
            
            return;
        }
        
        float acceleration = userInput.GetAcceleration();
        
        backLeftWheelCollider.motorTorque = acceleration * maxMotorForce;
        backRightWheelCollider.motorTorque = acceleration * maxMotorForce;
        
        backLeftWheelCollider.brakeTorque = 0;
        backRightWheelCollider.brakeTorque = 0;
    }

    private void Turn()
    {
        float turn = userInput.GetTurn();
        float angle = maxSteerAngle * turn;
        
        frontWheelCollider.steerAngle = angle;
        frontHiddenLeftWheelCollider.steerAngle = angle;
        frontHiddenRightWheelCollider.steerAngle = angle;
    }

    #endregion

    #region Breaking

    private void GetBreakingInput()
    {
        isBreaking = userInput.GetBreaking();
        
        player.SetCurrentState(isBreaking ? PlayerState.Braking : PlayerState.Running);
    }

    #endregion
    
    #region Wheel Visuals

    private void UpdateWheelVisuals(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos = wheelTransform.transform.position;
        Quaternion rot = wheelTransform.transform.rotation;
        
        wheelCollider.GetWorldPose(out pos, out rot);
        
        wheelTransform.transform.position = pos;
        wheelTransform.transform.rotation = rot;
    }

    #endregion

    #region Getter

    internal Vector3 GetVelocity()
    {
        return rb.linearVelocity;
    }

    #endregion
}
