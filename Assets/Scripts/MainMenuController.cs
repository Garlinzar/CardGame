using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SimpleMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("Main Menu Buttons")]
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;

    [Header("Back Buttons")]
    [SerializeField] private Button optionsBackButton;
    [SerializeField] private Button creditsBackButton;

    [Header("Audio Settings")]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Toggle muteToggle;

    [Header("Scene Management")]
    [SerializeField] private string gameplaySceneName = "GameScene";

    private void Start()
    {
        // Set up main menu button listeners
        if (startGameButton) startGameButton.onClick.AddListener(OnStartGameClicked);
        if (optionsButton) optionsButton.onClick.AddListener(OnOptionsClicked);
        if (creditsButton) creditsButton.onClick.AddListener(OnCreditsClicked);
        if (quitButton) quitButton.onClick.AddListener(QuitGame);

        // Set up back button listeners
        if (optionsBackButton) optionsBackButton.onClick.AddListener(ReturnToMainMenu);
        if (creditsBackButton) creditsBackButton.onClick.AddListener(ReturnToMainMenu);

        // Set up audio control listeners
        if (musicVolumeSlider) musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        if (sfxVolumeSlider) sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        if (muteToggle) muteToggle.onValueChanged.AddListener(ToggleMute);

        // Initialize audio settings from PlayerPrefs
        InitializeAudioSettings();

        // Show main menu panel by default
        ShowPanel(mainMenuPanel);
    }

    private void InitializeAudioSettings()
    {
        // Load saved audio settings or use defaults
        if (musicVolumeSlider)
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        
        if (sfxVolumeSlider)
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        
        if (muteToggle)
            muteToggle.isOn = PlayerPrefs.GetInt("Muted", 0) == 1;
    }

    private void OnStartGameClicked()
    {
        Debug.Log("Starting game: " + gameplaySceneName);
        // Load the game scene
        SceneManager.LoadScene(gameplaySceneName);
    }

    private void OnOptionsClicked()
    {
        ShowPanel(optionsPanel);
    }

    private void OnCreditsClicked()
    {
        ShowPanel(creditsPanel);
    }

    private void ReturnToMainMenu()
    {
        ShowPanel(mainMenuPanel);
    }

    private void ShowPanel(GameObject panelToShow)
    {
        // Hide all panels
        if (mainMenuPanel) mainMenuPanel.SetActive(false);
        if (optionsPanel) optionsPanel.SetActive(false);
        if (creditsPanel) creditsPanel.SetActive(false);

        // Show the selected panel
        if (panelToShow) panelToShow.SetActive(true);
    }

    private void SetMusicVolume(float volume)
    {
        // Save music volume setting
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
        
        // Here you would also set the actual audio source volume
        // Example: AudioManager.Instance.SetMusicVolume(volume);
        Debug.Log("Music volume set to: " + volume);
    }

    private void SetSFXVolume(float volume)
    {
        // Save SFX volume setting
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
        
        // Here you would also set the actual audio source volume
        // Example: AudioManager.Instance.SetSFXVolume(volume);
        Debug.Log("SFX volume set to: " + volume);
    }

    private void ToggleMute(bool isMuted)
    {
        // Save mute setting
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
        
        // Here you would also mute/unmute the actual audio
        // Example: AudioManager.Instance.SetMute(isMuted);
        Debug.Log("Audio muted: " + isMuted);
    }

    private void QuitGame()
    {
        Debug.Log("Quitting game");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}