using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadDefaultScenes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("MainRoom", LoadSceneMode.Additive);
        SceneManager.LoadScene("Basement", LoadSceneMode.Additive);
    }
    
}
