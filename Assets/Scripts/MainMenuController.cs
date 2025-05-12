using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SimpleMenuController : MonoBehaviour
{
    [Header("Main Menu Buttons")]
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;

    [Header("Scene Management")]
    [SerializeField] private string gameplaySceneName = "GameScene";

    private void Start()
    {
        // Set up button listeners
        if (startGameButton) startGameButton.onClick.AddListener(OnStartGameClicked);
        if (optionsButton) optionsButton.onClick.AddListener(OnOptionsClicked);
        if (creditsButton) creditsButton.onClick.AddListener(OnCreditsClicked);
        if (quitButton) quitButton.onClick.AddListener(QuitGame);
    }

    private void OnStartGameClicked()
    {
        Debug.Log("Start Game clicked! Would normally load: " + gameplaySceneName);
        // Uncomment the line below when you have a game scene created and added to build settings
        // SceneManager.LoadScene(gameplaySceneName);
    }

    private void OnOptionsClicked()
    {
        Debug.Log("Options button clicked! Would show Options Panel");
        // This will just log a message for now, we'll add panel switching later
    }

    private void OnCreditsClicked()
    {
        Debug.Log("Credits button clicked! Would show Credits Panel");
        // This will just log a message for now, we'll add panel switching later
    }

    private void QuitGame()
    {
        Debug.Log("Quit button clicked!");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}