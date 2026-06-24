using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("ChooseCharacter");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenProfile()
    {
        SceneManager.LoadScene("Profile");
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings");
    }
}
