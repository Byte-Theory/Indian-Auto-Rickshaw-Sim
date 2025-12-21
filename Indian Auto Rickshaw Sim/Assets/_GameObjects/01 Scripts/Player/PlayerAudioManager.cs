using System;
using System.Collections;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource idlingAudioSource;
    [SerializeField] private AudioSource[] drivingAudioSource;
    private float drivingAudioDur;
    private int playingDrivingAudioIdx;

    private bool isPlayerDriving = false;
    
    private Coroutine[] drivingAudioRoutine;

    private void Awake()
    {
        drivingAudioRoutine = new Coroutine[drivingAudioSource.Length];
    }

    public void SetPlayerDriving(bool isPlayerDriving, bool overrideDriving = false)
    {
        if ((this.isPlayerDriving == isPlayerDriving) && !overrideDriving)
        {
            return;
        }
        
        this.isPlayerDriving = isPlayerDriving;

        if (!isPlayerDriving)
        {
            foreach (AudioSource audioSource in drivingAudioSource)
            {
                audioSource.Stop();
            }
            
            idlingAudioSource.Play();

            for (int idx = 0; idx < drivingAudioRoutine.Length; idx++)
            {
                if (drivingAudioRoutine[idx] != null)
                {
                    StopCoroutine(drivingAudioRoutine[idx]);
                }
            }
        }
        else
        {
            idlingAudioSource.Stop();
            
            playingDrivingAudioIdx = 0;
            
            drivingAudioSource[playingDrivingAudioIdx].Play();
            drivingAudioDur = drivingAudioSource[playingDrivingAudioIdx].clip.length;

            drivingAudioRoutine[playingDrivingAudioIdx] = StartCoroutine(PlayNextDrivingAudioSource(drivingAudioDur * 0.9f));
        }
    }

    private IEnumerator PlayNextDrivingAudioSource(float duration)
    {
        yield return new WaitForSeconds(duration);

        playingDrivingAudioIdx++;
        if (playingDrivingAudioIdx >= drivingAudioSource.Length)
        {
            playingDrivingAudioIdx = 0;
        }
        
        drivingAudioSource[playingDrivingAudioIdx].Play();
        drivingAudioDur = drivingAudioSource[playingDrivingAudioIdx].clip.length;
        
        drivingAudioRoutine[playingDrivingAudioIdx] = StartCoroutine(PlayNextDrivingAudioSource(drivingAudioDur * 0.75f));
    }
}
