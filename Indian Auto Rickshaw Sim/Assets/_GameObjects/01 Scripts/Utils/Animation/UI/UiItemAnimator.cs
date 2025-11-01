using System;
using UnityEngine;

public class UiItemAnimator : MonoBehaviour
{
    private UiItemScaleAnimator[] uiItemScaleAnimators;
    private UiItemColorAnimator[] uiItemColorAnimator;

    private void Awake()
    {
        uiItemScaleAnimators = GetComponentsInChildren<UiItemScaleAnimator>();
        uiItemColorAnimator = GetComponentsInChildren<UiItemColorAnimator>();
    }

    private void OnEnable()
    {
        PlayEntry(true);
    }

    public void PlayEntry(bool isOnEnabledPlaying = false)
    {
        for (int i = 0; i < uiItemScaleAnimators.Length; i++)
        {
            uiItemScaleAnimators[i].TriggerAnimation(true, isOnEnabledPlaying);
        }
        
        for (int i = 0; i < uiItemColorAnimator.Length; i++)
        {
            uiItemColorAnimator[i].TriggerAnimation(true, isOnEnabledPlaying);
        }
    }
    
    public void PlayExit()
    {
        for (int i = 0; i < uiItemScaleAnimators.Length; i++)
        {
            uiItemScaleAnimators[i].TriggerAnimation(false);
        }
        
        for (int i = 0; i < uiItemColorAnimator.Length; i++)
        {
            uiItemColorAnimator[i].TriggerAnimation(false);
        }
    }
}
