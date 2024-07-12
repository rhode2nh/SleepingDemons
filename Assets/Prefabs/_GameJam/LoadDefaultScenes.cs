using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadDefaultScenes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #if !UNITY_EDITOR
        SceneManager.LoadScene("Apartment", LoadSceneMode.Additive);
        #endif
    }
    
}
