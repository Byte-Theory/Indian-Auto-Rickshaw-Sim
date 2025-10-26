using UnityEngine;

public class SimpleVehicleShake : MonoBehaviour
{
    [Header("Shake Settings")]
    [Tooltip("Overall intensity of the shake")]
    public float shakeIntensity = 0.02f;

    [Tooltip("Speed of the shake oscillation")]
    public float shakeSpeed = 20f;

    [Tooltip("How much the shake varies on each axis")]
    public Vector3 shakeAmount = new Vector3(1f, 0.5f, 1f);

    private Vector3 originalPos;
    private Quaternion originalRot;

    void Start()
    {
        originalPos = transform.localPosition;
        originalRot = transform.localRotation;
    }

    void Update()
    {
        float time = Time.time * shakeSpeed;

        // Small sinusoidal oscillations
        float x = Mathf.Sin(time * 1.1f) * shakeAmount.x;
        float y = Mathf.Sin(time * 1.7f) * shakeAmount.y;
        float z = Mathf.Sin(time * 2.3f) * shakeAmount.z;

        // Apply shake to position
        transform.localPosition = originalPos + new Vector3(x, y, z) * shakeIntensity;

        // Optional small rotational jitter
        transform.localRotation = originalRot * Quaternion.Euler(
            y * 5f * shakeIntensity,
            x * 5f * shakeIntensity,
            z * 5f * shakeIntensity
        );
    }

    void OnDisable()
    {
        // Reset when disabled
        transform.localPosition = originalPos;
        transform.localRotation = originalRot;
    }
}