using UnityEngine;
using UnityEngine.UI; // Required for UI elements like Button
using UnityEngine.SceneManagement;

public class IngameSettingsController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject ingameSettingsPanel;

    [Header("Buttons")]
    // Button that opens the settings panel (located outside the panel)
    [SerializeField] private Button openSettingsButton;

    // Buttons located inside the settings panel
    [SerializeField] private Button playButton; // This will close the panel
    [SerializeField] private Button settingsButton; // This goes to Main Menu's options
    [SerializeField] private Button backToMainMenuButton;

    [Header("Scene Management")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void Awake() // Using Awake to ensure listeners are set up before Start
    {
        // --- Set up button listeners ---

        // Listener for the button that opens the settings menu
        if (openSettingsButton)
        {
            openSettingsButton.onClick.AddListener(ShowIngameSettings);
        }

        // Listener for the 'Play' button to close the panel
        if (playButton)
        {
            playButton.onClick.AddListener(HideIngameSettings);
        }

        // Listener for the 'Settings' button to go to the Main Menu options
        if (settingsButton)
        {
            settingsButton.onClick.AddListener(GoToMainMenuAndOpenOptions);
        }

        // Listener for the 'Back to Main Menu' button
        if (backToMainMenuButton)
        {
            backToMainMenuButton.onClick.AddListener(BackToMainMenu);
        }
    }

    private void Start()
    {
        // Ensure the settings panel is inactive when the scene starts
        if (ingameSettingsPanel != null)
        {
            ingameSettingsPanel.SetActive(false);
        }
    }

    // This method shows the in-game settings panel
    private void ShowIngameSettings()
    {
        if (ingameSettingsPanel != null)
        {
            ingameSettingsPanel.SetActive(true);
            // Optional: Pause the game when settings are open
            // Time.timeScale = 0f;
        }
    }

    // This method hides the in-game settings panel
    private void HideIngameSettings()
    {
        if (ingameSettingsPanel != null)
        {
            ingameSettingsPanel.SetActive(false);
            // Optional: Resume the game when settings are closed
            // Time.timeScale = 1f;
        }
    }

    // This method loads the Main Menu and signals it to open the options panel
    private void GoToMainMenuAndOpenOptions()
    {
        // Optional: Ensure time scale is reset before leaving the scene
        // Time.timeScale = 1f;

        // Use PlayerPrefs to communicate the intent to open the options panel
        PlayerPrefs.SetInt("OpenOptionsPanel", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // This method loads the Main Menu scene directly
    private void BackToMainMenu()
    {
        // Optional: Ensure time scale is reset before leaving the scene
        // Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}