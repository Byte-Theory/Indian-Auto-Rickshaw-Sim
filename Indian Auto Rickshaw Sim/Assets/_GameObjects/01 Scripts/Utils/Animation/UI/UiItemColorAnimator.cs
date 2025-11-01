using UnityEngine;
using UnityEngine.UI;

public class UiItemColorAnimator : MonoBehaviour
{
    [SerializeField] private bool selfTrigger = false;
    
    [Header("Animation Data")]
    [SerializeField] private bool playOnEnable = false;
    [SerializeField] private bool loop = false;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Color startColor = Color.white;
    [SerializeField] private Color endColor = Color.white;
    
    // Animation
    private bool isAnimating = false;
    private bool isEntry = false;
    private float elapsedTime = 0.0f;
    
    // Rect T
    private Image targetImage;
    
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
            elapsedTime += Time.deltaTime;
            
            float fac = elapsedTime / duration;
            
            Color color = CalcColor();
            targetImage.color = color;

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
        if (targetImage == null)
        {
            targetImage = GetComponent<Image>();
        }
        
        isAnimating = true;
        this.isEntry = isEntry;

        elapsedTime = 0.0f;

        Color color = CalcColor();
        targetImage.color = color;
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
        
        if (targetImage == null)
        {
            targetImage = GetComponent<Image>();
        }
        
        isAnimating = true;
        this.isEntry = isEntry;

        elapsedTime = 0.0f;

        Color color = CalcColor();
        targetImage.color = color;
    }

    private Color CalcColor()
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
        
        fac = isEntry ? fac : 1.0f - fac;
        
        Color color = Color.Lerp(startColor, endColor, fac);

        return color;
    }
}
