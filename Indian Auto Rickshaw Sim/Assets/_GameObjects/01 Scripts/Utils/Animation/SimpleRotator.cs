using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("Rotation direction (e.g. 0,1,0 = Y axis)")]
    [SerializeField] private Vector3 rotationDirection = Vector3.up;

    [Tooltip("Rotation speed in degrees per second")]
    [SerializeField] private float rotationSpeed = 90f;

    [Tooltip("Rotate in local space or world space")]
    [SerializeField] private bool useLocalSpace = true;

    private void Update()
    {
        Vector3 rotation = rotationDirection.normalized * (rotationSpeed * Time.deltaTime);

        transform.Rotate(rotation, useLocalSpace ? Space.Self : Space.World);
    }
}