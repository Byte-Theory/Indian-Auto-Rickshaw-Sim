using UnityEngine;

public class SimplePositionOscillator : MonoBehaviour
{
    [Header("Position Settings")]
    [Tooltip("Start position")]
    [SerializeField] private Vector3 positionA;

    [Tooltip("End position")]
    [SerializeField] private Vector3 positionB;

    [Header("Oscillation")]
    [Tooltip("Time (in seconds) to move from A to B")]
    [SerializeField] private float duration = 1f;

    [Tooltip("Use local space instead of world space")]
    [SerializeField] private bool useLocalSpace = true;

    [Header("Easing")]
    [Tooltip("Ease in and out curve")]
    public AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private float time;

    private void Update()
    {
        if (duration <= 0f)
            return;

        time += Time.deltaTime;

        // PingPong creates back-and-forth motion
        float t = Mathf.PingPong(time / duration, 1f);
        float easedT = easeCurve.Evaluate(t);

        Vector3 pos = Vector3.Lerp(positionA, positionB, easedT);

        if (useLocalSpace)
        {
            transform.localPosition = pos;
        }
        else
        {
            transform.position = pos;
        }
    }
}