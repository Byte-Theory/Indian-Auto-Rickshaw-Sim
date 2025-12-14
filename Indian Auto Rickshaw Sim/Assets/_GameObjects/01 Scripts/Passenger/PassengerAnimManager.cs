using UnityEngine;

public class PassengerAnimManager : MonoBehaviour
{
    private Animator anim;
    
    private readonly string IdleAnimTag = "Idle";
    private readonly string IdleAnimIdxTag = "IdleAnimIdx";
    private readonly int TotalIdleAnimCt = 5;
    
    private readonly string WalkAnimTag = "Walk";
    private readonly string WalkAnimIdxTag = "WalkAnimIdx";
    private readonly int TotalWalkAnimCt = 4;
    
    private readonly string WaveAnimTag = "Wave";
    private readonly string SittingInAutoAnimTag = "SittingInAuto";

    #region SetUp

    public void SetUp()
    {
        anim = GetComponentInChildren<Animator>();
    }

    #endregion
    
    public void PlayIdleAnim()
    {
        ResetAllAnim();
        
        int randIdx = Random.Range(0, TotalIdleAnimCt);
        float fac = (1.0f * randIdx) / (TotalIdleAnimCt - 1);
        
        anim.SetBool(IdleAnimTag, true);
        anim.SetFloat(IdleAnimIdxTag, fac);
    }
    
    public void PlayWalkAnim()
    {
        ResetAllAnim();
        
        int randIdx = Random.Range(0, TotalWalkAnimCt);
        float fac = (1.0f * randIdx) / (TotalWalkAnimCt - 1);
        
        anim.SetBool(WalkAnimTag, true);
        anim.SetFloat(WalkAnimIdxTag, fac);
    }
    
    public void PlayWaveAnim()
    {
        ResetAllAnim();
        
        anim.SetBool(WaveAnimTag, true);
    }
    
    public void PlaySettingInAutoAnim()
    {
        ResetAllAnim();
        
        anim.SetBool(SittingInAutoAnimTag, true);
    }
    
    private void ResetAllAnim()
    {
        anim.SetBool(IdleAnimTag, false);
        anim.SetBool(WalkAnimTag, false);
        anim.SetBool(WaveAnimTag, false);
        anim.SetBool(SittingInAutoAnimTag, false);
    }

    #region Optimization

    public void EnableAnimations()
    {
        anim.enabled = true;
    }

    public void DisableAnimations()
    {
        anim.enabled = false;
    }

    #endregion
}
