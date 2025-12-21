using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioClipType
{
    Unknown = -1,
    ButtonClick,
    PassengerPickup,
    PassengerDropped,
    MoneySpent,
    MissionClaim,
    AutoHit
}

[System.Serializable]
public class AudioData
{
    public AudioClipType audioClipType;
    public AudioClip audioClip;
    public float volume;
}

public class AudioManager : MonoBehaviour
{
    [Header("Audio Data")]
    [SerializeField] private List<AudioData> allAudioData;
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgAudioSource;
    [SerializeField] private AudioSource[] allAudioSources;
    private float[] lastPlayedTime;
    
    [Header("Mixer")]
    [SerializeField] private AudioMixer audioMixer;
    
    #region Singleton

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
        
        bgAudioSource.Play();
        
        lastPlayedTime = new float[allAudioData.Count];
        for (int idx = 0; idx < lastPlayedTime.Length; idx++)
        {
            lastPlayedTime[idx] = 0;
        }

        SetVolume(0);
    }

    #endregion

    public void PlayAudio(AudioClipType audioClipType, bool useRandomPitch = false)
    {
        AudioSource freeSource = null;
        for (int idx = 0; idx < allAudioSources.Length; idx++)
        {
            if (!allAudioSources[idx].isPlaying)
            {
                freeSource = allAudioSources[idx];
                lastPlayedTime[idx] = Time.time;
                break;
            }
        }

        if (freeSource == null)
        {
            int oldestIndex = 0;
            float maxTime = float.MinValue;
            for (int idx = 0; idx < lastPlayedTime.Length; idx++)
            {
                if (lastPlayedTime[idx] > maxTime)
                {
                    maxTime = lastPlayedTime[idx];
                    oldestIndex =idx;
                }
            }
            
            freeSource = allAudioSources[oldestIndex];
        }

        if (freeSource == null)
        {
            return;
        }
        
        AudioClip audioClip = null;
        float volume = 0;
        for (int idx = 0; idx < allAudioData.Count; idx++)
        {
            if (allAudioData[idx].audioClipType == audioClipType)
            {
                audioClip = allAudioData[idx].audioClip;
                volume = allAudioData[idx].volume;
            }
        }
        
        if (audioClip == null)
        {
            return;
        }
        
        freeSource.clip = audioClip;
        freeSource.volume = volume;

        if (useRandomPitch)
        {
            freeSource.pitch = Random.Range(0.9f, 1.1f);
        }
        else
        {
            freeSource.pitch = 1.0f;
        }
        
        freeSource.Play();
    }
    
    public void SetVolume(float value)
    {
        audioMixer.SetFloat("Volume", value);
    }
}
