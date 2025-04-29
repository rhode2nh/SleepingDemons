using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private string _managers;
    [SerializeField] private string _ui;
    [SerializeField] private string _fpsController;
    [SerializeField] private string _level;

    void Awake()
    {
        SceneManager.LoadScene(_managers, LoadSceneMode.Additive);
        SceneManager.LoadScene(_ui, LoadSceneMode.Additive);
        StartCoroutine(LoadAndSetActiveScene(_level));
    }

    private static IEnumerator LoadAndSetActiveScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Scene loadedScene = SceneManager.GetSceneByName(sceneName);

        if (loadedScene.IsValid())
        {
            SceneManager.SetActiveScene(loadedScene);
        }
        else
        {
            Debug.LogError($"Failed to set {sceneName} as the active scene.");
        }
    }
}
