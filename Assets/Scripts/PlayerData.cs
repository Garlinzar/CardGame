[System.Serializable]
public class PlayerData
{
    // Permanenter Fortschritt
    public bool campaignCompleted;
    public int highScore;
    public int souls;

    public int greedLevel;
    public int rageLevel;
    public int gluttonyLevel;

    // Fortschritt (aktueller Run Campaign)
    public int gold;
    public int wave;
    public float playerHP;


    // Fortschritt (aktueller Run Infinite)
    public int goldInf;
    public int waveInf;
    public float playerHPInf;
}
