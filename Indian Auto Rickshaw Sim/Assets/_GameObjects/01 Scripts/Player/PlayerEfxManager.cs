using UnityEngine;

public class PlayerEfxManager : MonoBehaviour
{
    [Header("Tween")]
    [SerializeField] private Tween containerTween;

    [Header("Efx")] 
    [SerializeField] private ParticleSystem passengerCollectedEfx;
    
    #region Set Up

    public void SetUp()
    {
    }

    #endregion

    #region Ride Efx

    public void PlayPassengerCollectedEfx()
    {
        passengerCollectedEfx.Play();
        containerTween.PlayTween("PassangerCollected");
        
        AudioManager.Instance.PlayAudio(AudioClipType.PassengerPickup);
    }
    
    public void PlayRideCompleteEfx()
    {
        Vector3 pos = transform.position;
        pos += transform.forward * 1.5f;  
        pos.y += 2.5f;
            
        GameObject efxGo = ObjectPooler.Instance.SpawnFromPool(2, pos, Quaternion.identity);
        efxGo.GetComponent<ParticleSystem>().Play();
        
        AudioManager.Instance.PlayAudio(AudioClipType.PassengerDropped);
    }

    #endregion
}
