using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettingsButton : MonoBehaviour
{
    public void OpenIngameMenu()
    {
        SceneManager.LoadScene("Ingame Menu");
    }
}
