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
        SceneManager.LoadScene(_fpsController, LoadSceneMode.Additive);
        SceneManager.LoadScene(_level, LoadSceneMode.Additive);
    }
}
