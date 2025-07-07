using UnityEngine;
using TMPro; // For displaying upgrade level and cost

public class UpgradeManager : MonoBehaviour
{
    // Assign in Inspector: Reference to your existing Currency script
    [SerializeField] private Currency currency;

    [Header("Upgrade Buttons & UI")]
    [SerializeField] private GameObject greedUpgradeButton;
    [SerializeField] private GameObject wrathUpgradeButton;
    [SerializeField] private GameObject gluttonyUpgradeButton;

    [Header("Upgrade Cost Text (optional, for display)")]
    [SerializeField] private TMP_Text greedCostText;
    [SerializeField] private TMP_Text wrathCostText;
    [SerializeField] private TMP_Text gluttonyCostText;

    [Header("Upgrade Level Text (optional, for display)")]
    [SerializeField] private TMP_Text greedLevelText;
    [SerializeField] private TMP_Text wrathLevelText;
    [SerializeField] private TMP_Text gluttonyLevelText;

    // Base cost for all upgrades
    private const int BASE_COST = 10;
    // PlayerPrefs keys for upgrade levels
    private const string GREED_LEVEL_KEY = "UpgradeGreedLevel";
    private const string WRATH_LEVEL_KEY = "UpgradeWrathLevel";
    private const string GLUTTONY_LEVEL_KEY = "UpgradeGluttonyLevel";

    void Start()
    {
        // Load existing levels or set to 0 if not found
        // Update UI for all upgrades
        UpdateUpgradeUI();
    }

    /// <summary>
    /// Calculates the cost of an upgrade based on its current level.
    /// The cost starts at BASE_COST and doubles for each subsequent level.
    /// </summary>
    /// <param name="currentLevel">The current level of the upgrade.</param>
    /// <returns>The calculated cost in souls.</returns>
    private int CalculateUpgradeCost(int currentLevel)
    {
        // Cost starts at BASE_COST for level 0, then doubles for each level
        if (currentLevel == 0)
        {
            return BASE_COST;
        }
        else
        {
            // The cost for level N is BASE_COST * (2^(N))
            // Example: Level 0 cost 10, Level 1 cost 20, Level 2 cost 40, etc.
            return BASE_COST * (1 << currentLevel); // Bit shift is a fast way to calculate powers of 2
        }
    }

    /// <summary>
    /// Handles the purchase of the Greed upgrade.
    /// </summary>
    public void BuyGreedUpgrade()
    {
        int currentLevel = PlayerPrefs.GetInt(GREED_LEVEL_KEY, 0);
        int cost = CalculateUpgradeCost(currentLevel);

        if (currency.SpendSoul(cost)) // Try to spend souls
        {
            PlayerPrefs.SetInt(GREED_LEVEL_KEY, currentLevel + 1); // Increment level
            PlayerPrefs.Save(); // Save PlayerPrefs immediately
            Debug.Log($"Greed upgraded to level {currentLevel + 1} for {cost} souls.");
            UpdateUpgradeUI(); // Update UI after purchase
        }
    }

    /// <summary>
    /// Handles the purchase of the Wrath upgrade.
    /// </summary>
    public void BuyWrathUpgrade()
    {
        int currentLevel = PlayerPrefs.GetInt(WRATH_LEVEL_KEY, 0);
        int cost = CalculateUpgradeCost(currentLevel);

        if (currency.SpendSoul(cost))
        {
            PlayerPrefs.SetInt(WRATH_LEVEL_KEY, currentLevel + 1);
            PlayerPrefs.Save();
            Debug.Log($"Wrath upgraded to level {currentLevel + 1} for {cost} souls.");
            UpdateUpgradeUI();
        }
    }

    /// <summary>
    /// Handles the purchase of the Gluttony upgrade.
    /// </summary>
    public void BuyGluttonyUpgrade()
    {
        int currentLevel = PlayerPrefs.GetInt(GLUTTONY_LEVEL_KEY, 0);
        int cost = CalculateUpgradeCost(currentLevel);

        if (currency.SpendSoul(cost))
        {
            PlayerPrefs.SetInt(GLUTTONY_LEVEL_KEY, currentLevel + 1);
            PlayerPrefs.Save();
            Debug.Log($"Gluttony upgraded to level {currentLevel + 1} for {cost} souls.");
            UpdateUpgradeUI();
        }
    }

    /// <summary>
    /// Updates all upgrade UI elements (cost and level).
    /// </summary>
    private void UpdateUpgradeUI()
    {
        // Greed
        int greedLevel = PlayerPrefs.GetInt(GREED_LEVEL_KEY, 0);
        int greedCost = CalculateUpgradeCost(greedLevel);
        if (greedCostText != null) greedCostText.text = $"{greedCost} Souls";
        if (greedLevelText != null) greedLevelText.text = $"Level: {greedLevel}";

        // Wrath
        int wrathLevel = PlayerPrefs.GetInt(WRATH_LEVEL_KEY, 0);
        int wrathCost = CalculateUpgradeCost(wrathLevel);
        if (wrathCostText != null) wrathCostText.text = $"{wrathCost} Souls";
        if (wrathLevelText != null) wrathLevelText.text = $"Level: {wrathLevel}";

        // Gluttony
        int gluttonyLevel = PlayerPrefs.GetInt(GLUTTONY_LEVEL_KEY, 0);
        int gluttonyCost = CalculateUpgradeCost(gluttonyLevel);
        if (gluttonyCostText != null) gluttonyCostText.text = $"{gluttonyCost} Souls";
        if (gluttonyLevelText != null) gluttonyLevelText.text = $"Level: {gluttonyLevel}";
    }

    /// <summary>
    /// Returns the current level of a specific upgrade.
    /// Use this from other scripts that need to know the upgrade level.
    /// </summary>
    public int GetUpgradeLevel(string upgradeKey)
    {
        switch (upgradeKey)
        {
            case "Greed":
                return PlayerPrefs.GetInt(GREED_LEVEL_KEY, 0);
            case "Wrath":
                return PlayerPrefs.GetInt(WRATH_LEVEL_KEY, 0);
            case "Gluttony":
                return PlayerPrefs.GetInt(GLUTTONY_LEVEL_KEY, 0);
            default:
                Debug.LogError($"Invalid upgrade key: {upgradeKey}");
                return 0;
        }
    }

    /// <summary>
    /// Resets all upgrade levels and updates the UI.
    /// Useful for debugging or starting a new game.
    /// </summary>
    public void ResetAllUpgrades()
    {
        PlayerPrefs.DeleteKey(GREED_LEVEL_KEY);
        PlayerPrefs.DeleteKey(WRATH_LEVEL_KEY);
        PlayerPrefs.DeleteKey(GLUTTONY_LEVEL_KEY);
        PlayerPrefs.Save();
        Debug.Log("All permanent upgrades reset.");
        UpdateUpgradeUI();
    }
}