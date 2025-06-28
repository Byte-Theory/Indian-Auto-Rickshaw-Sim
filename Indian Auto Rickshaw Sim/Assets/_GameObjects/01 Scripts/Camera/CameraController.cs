using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] public Transform targetT;
    [SerializeField] public Transform rigT;
    [SerializeField] public Transform pivotT;

    [Header("Rig Follow Data")] 
    [SerializeField] private Vector3 rigFollowOffset;
    [SerializeField] private float rigFollowSpeed;
    [SerializeField] private float rigRotationSpeed;
    
    [Header("Pivot Offset Data")]
    [SerializeField] private Vector3 pivotOffset;
    [SerializeField] private Vector3 pivotCloseOffset;
    [SerializeField] private Vector3 pivotFarOffset;
    [SerializeField] private float pivotMoveSpeed;
    
    [Header("Pivot Rotation Data")]
    [SerializeField] private Vector3 pivotRotationOffset;
    [SerializeField] private Vector3 pivotCloseRotationOffset;
    [SerializeField] private Vector3 pivotFarRotationOffset;
    [SerializeField] private float pivotRotationSpeed;

    [Header("Pivot Commons")]
    [SerializeField] private float pivotOffsetUpdateSpeed;
    
    // Ref
    private Player player;
    
    void Start()
    {
        player = targetT.GetComponent<Player>();
        
        SetUpRig();
        SetUpPivot();
    }

    void LateUpdate()
    {
        FollowRigToTarget();
        RotateRigToTarget();

        SelectPivotPosOffset();
        MovePivot();
        RotatePivot();
    }

    #region Rig

    private void SetUpRig()
    {
        Vector3 desiredPosition = targetT.position + rigFollowOffset;
        rigT.position = desiredPosition;
        
        Vector3 desiredRotation = rigT.rotation.eulerAngles;
        desiredRotation.y = targetT.rotation.eulerAngles.y;
        
        rigT.rotation = Quaternion.Euler(desiredRotation);
    }
    
    private void FollowRigToTarget()
    {
        Vector3 desiredPosition = targetT.position + rigFollowOffset;
        
        rigT.position = Vector3.Lerp(rigT.position, desiredPosition, Time.deltaTime * rigFollowSpeed);
    }
    
    private void RotateRigToTarget()
    {
        Vector3 desiredRotation = rigT.rotation.eulerAngles;
        desiredRotation.y = targetT.rotation.eulerAngles.y;
        
        Quaternion desiredRot = Quaternion.Euler(desiredRotation);
        
        rigT.rotation = Quaternion.Lerp(rigT.rotation, desiredRot, Time.deltaTime * rigRotationSpeed);
    }

    #endregion

    #region Pivot

    private void SetUpPivot()
    {
        pivotOffset = pivotFarOffset;
        pivotRotationOffset = pivotFarRotationOffset;
        
        pivotT.localPosition = pivotOffset;
        
        Quaternion desiredRotation = Quaternion.Euler(pivotRotationOffset);
        pivotT.localRotation = desiredRotation;
    }
    
    private void MovePivot()
    {
        pivotT.localPosition = Vector3.Lerp(pivotT.localPosition, pivotOffset, Time.deltaTime * pivotMoveSpeed);
    }

    private void RotatePivot()
    {
        Quaternion desiredRotation = Quaternion.Euler(pivotRotationOffset);
        pivotT.localRotation = Quaternion.Lerp(pivotT.localRotation, desiredRotation, Time.deltaTime * pivotRotationSpeed);
    }

    private void SelectPivotPosOffset()
    {
        PlayerState playerState = player.GetPlayerState();
        Vector3 velocity = player.engine.GetVelocity();
        Vector3 playerForward = player.transform.forward;
        
        float dot = Vector3.Dot(velocity, playerForward);

        Vector3 finalOffset;
        Vector3 finalRotationOffset;
        
        if (playerState == PlayerState.Braking || dot < -0.75f)
        {
            finalOffset = pivotCloseOffset;
            finalRotationOffset = pivotCloseRotationOffset;
        }
        else
        {
            finalOffset = pivotFarOffset;
            finalRotationOffset = pivotFarRotationOffset;
        }
        
        pivotOffset = Vector3.Lerp(pivotOffset, finalOffset, Time.deltaTime * pivotOffsetUpdateSpeed);
        pivotRotationOffset = Vector3.Lerp(pivotRotationOffset, finalRotationOffset, Time.deltaTime * pivotOffsetUpdateSpeed);
    }

    #endregion
}
