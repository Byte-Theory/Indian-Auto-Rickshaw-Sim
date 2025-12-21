using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseMenuGo;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button closeGameButton;

    // Ref
    private GameplayTimeManager gameplayTimeManager;
    
    private void Awake()
    {
        pauseButton.onClick.AddListener(OnClickPauseButton);
        resumeButton.onClick.AddListener(OnClickResumeButton);
        homeButton.onClick.AddListener(OnClickHomeButton);
        closeGameButton.onClick.AddListener(OnClickCloseGameButton);
        
        volumeSlider.onValueChanged.AddListener(OnVolumeSliderChange);
        
        pauseMenuGo.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameplayTimeManager = GameplayTimeManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickPauseButton();
        }
    }

    private void OnVolumeSliderChange(float value)
    {
        AudioManager.Instance.SetVolume(value);
    }
    
    #region Buttons

    private void OnClickPauseButton()
    {
        AudioManager.Instance.PlayAudio(AudioClipType.ButtonClick);
        
        pauseMenuGo.SetActive(true);
        gameplayTimeManager.PauseGame();
    }

    private void OnClickResumeButton()
    {
        AudioManager.Instance.PlayAudio(AudioClipType.ButtonClick);
        
        pauseMenuGo.SetActive(false);
        gameplayTimeManager.ResumeGame();
    }
    
    private void OnClickHomeButton()
    {
        AudioManager.Instance.PlayAudio(AudioClipType.ButtonClick);
        SceneManager.LoadScene(0);
    }
    
    private void OnClickCloseGameButton()
    {
        Application.Quit();
    }

    #endregion
}
