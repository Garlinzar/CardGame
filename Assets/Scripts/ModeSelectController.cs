using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ModeSelectController : MonoBehaviour
{
    [Header("Mode Select Buttons")]
    [SerializeField] private Button campaignModeButton;
    [SerializeField] private Button infiniteModeButton;
    [SerializeField] private Button MainMenuButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button ReturnButton;
    [SerializeField] private GameObject upgradesPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Make sure the upgrades panel is deactivated at start
        if (upgradesPanel) upgradesPanel.SetActive(false);

        // Add button listeners
        if (campaignModeButton) campaignModeButton.onClick.AddListener(OnCampaignModeClicked);
        if (infiniteModeButton) infiniteModeButton.onClick.AddListener(OnInfiniteModeClicked);
        if (MainMenuButton) MainMenuButton.onClick.AddListener(OnMainMenuClicked);
        if (shopButton) shopButton.onClick.AddListener(OnShopClicked);
        if (ReturnButton) ReturnButton.onClick.AddListener(CloseUpgradesPanel);
    }

    private void OnCampaignModeClicked()
    {
        // Load the campaign mode scene
        SceneManager.LoadScene("Main Scene");
    }

    private void OnInfiniteModeClicked()
    {
        // Infinite mode not implemented yet
        Debug.Log("Infinite Mode not implemented yet!");
    }

    private void OnMainMenuClicked()
    {
        // Load the main menu scene
        SceneManager.LoadScene("Main Menu");
    }

    private void OnShopClicked()
    {
        // Show the upgrades panel
        if (upgradesPanel)
        {
            upgradesPanel.SetActive(true);
        }
    }

    // Optional: Method to close the upgrades panel
    public void CloseUpgradesPanel()
    {
        if (upgradesPanel)
        {
            upgradesPanel.SetActive(false);
        }
    }
}