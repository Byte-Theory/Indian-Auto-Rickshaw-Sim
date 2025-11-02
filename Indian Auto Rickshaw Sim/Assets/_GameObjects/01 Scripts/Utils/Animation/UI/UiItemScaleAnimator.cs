using System;
using UnityEngine;

public class UiItemScaleAnimator : MonoBehaviour
{
    [SerializeField] private bool selfTrigger = false;
    [SerializeField] private bool useUnscaledTime = false;
    
    [Header("Animation Data")]
    [SerializeField] private bool playOnEnable = false;
    [SerializeField] private bool loop = false;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private AnimationCurve scaleCurve;
    
    // Animation
    private bool isAnimating = false;
    private bool isEntry = false;
    private float elapsedTime = 0.0f;
    
    // Rect T
    private RectTransform targetRectT;

    private void OnEnable()
    {
        if (selfTrigger)
        {
            SelfTriggerAnimation(true);
        }
    }

    private void Update()
    {
        if (isAnimating)
        {
            float delta = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            elapsedTime += delta;
            
            float fac = elapsedTime / duration;
            
            float scale = CalcScale();
            targetRectT.localScale = Vector3.one * scale;

            if (fac >= 1)
            {
                if (loop)
                {
                    isEntry = !isEntry;
                    elapsedTime = 0.0f;
                }
                else
                {
                    isAnimating = false;   
                }
            }
        }
    }

    private void SelfTriggerAnimation(bool isEntry)
    {
        if (targetRectT == null)
        {
            targetRectT = GetComponent<RectTransform>();
        }
        
        isAnimating = true;
        this.isEntry = isEntry;

        elapsedTime = 0.0f;

        float scale = CalcScale();
        targetRectT.localScale = Vector3.one * scale;
    }
    
    public void TriggerAnimation(bool isEntry, bool isOnEnabledPlaying = false)
    {
        if (selfTrigger)
        {
            return;
        }
        
        if (isOnEnabledPlaying && !playOnEnable)
        {
            return;
        }
        
        if (targetRectT == null)
        {
            targetRectT = GetComponent<RectTransform>();
        }
        
        isAnimating = true;
        this.isEntry = isEntry;

        elapsedTime = 0.0f;

        float scale = CalcScale();
        targetRectT.localScale = Vector3.one * scale;
    }

    private float CalcScale()
    {
        float fac = elapsedTime / duration;

        if (fac <= 0)
        {
            fac = 0;
        }
        else if (fac >= 1)
        {
            fac = 1;
        }
        
        float time = isEntry ? fac : 1.0f - fac;
        float scale = scaleCurve.Evaluate(time);

        return scale;
    }
}
