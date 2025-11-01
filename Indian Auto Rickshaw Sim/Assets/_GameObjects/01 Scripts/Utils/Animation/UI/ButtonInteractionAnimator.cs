using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
public class ButtonInteractionAnimator : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private RectTransform targetRectT;
    
    private Button button;
    private Vector3 originalScale;
    private Vector3 targetScale;

    private bool isAnimating = false;
    private float animDur = 0.25f;
    private float curAnimTimeElapsed = 0.0f;
    private float pressedScale = 0.875f;
    private float scaleChangeSpeed = 10.0f;

    private void Awake()
    {
        if (targetRectT == null)
        {
            targetRectT = GetComponent<RectTransform>();
        }
        
        button = GetComponent<Button>();
        
        originalScale = targetRectT.localScale;
        targetScale = originalScale;

        var trigger = button.gameObject.AddComponent<EventTrigger>();

        AddEvent(trigger, EventTriggerType.PointerDown, OnPointerDown);
        AddEvent(trigger, EventTriggerType.PointerUp, OnPointerUp);
        AddEvent(trigger, EventTriggerType.PointerExit, OnPointerExit);
    }

    private void Update()
    {
        if (isAnimating)
        {
            curAnimTimeElapsed += Time.deltaTime;
            float fac = curAnimTimeElapsed / animDur;
            
            targetRectT.localScale = Vector3.Lerp(targetRectT.localScale, targetScale, Time.deltaTime * scaleChangeSpeed);

            if (fac >= 1)
            {
                isAnimating = false;
            }
        }
    }

    private void AddEvent(EventTrigger trigger, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }

    private void OnPointerDown(BaseEventData data)
    {
        targetScale = originalScale * pressedScale;

        isAnimating = true;
        curAnimTimeElapsed = 0.0f;
    }

    private void OnPointerUp(BaseEventData data)
    {
        targetScale = originalScale;
        
        isAnimating = true;
        curAnimTimeElapsed = 0.0f;
    }

    private void OnPointerExit(BaseEventData data)
    {
        targetScale = originalScale;
        
        isAnimating = true;
        curAnimTimeElapsed = 0.0f;
    }
}