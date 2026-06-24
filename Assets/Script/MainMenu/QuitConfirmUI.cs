using UnityEngine;

public class QuitConfirmUI : MonoBehaviour
{
    public GameObject quitConfirm; // 在 Inspector 中拖入 QuitConfirmPanel

    public void ShowConfirmPanel()
    {
        quitConfirm.SetActive(true);
    }

    public void HideConfirmPanel()
    {
        quitConfirm.SetActive(false);
    }

    public void ConfirmQuit()
    {
        Application.Quit();
    }
}