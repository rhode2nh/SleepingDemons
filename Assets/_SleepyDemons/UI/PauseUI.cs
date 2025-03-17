using UnityEngine;

public class PauseUI : MonoBehaviour, IUIPanel
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.OnOpenPauseUI += OpenPanel;
        UIManager.Instance.OnClosePauseUI += ClosePanel;
        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
