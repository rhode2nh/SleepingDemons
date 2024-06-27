using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneLoad : MonoBehaviour
{
    public bool loaded = false;
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            if (loaded == false) {
                SceneManager.LoadScene("TestBasement", LoadSceneMode.Additive);
                loaded = true;
            } else {
                SceneManager.UnloadSceneAsync("TestBasement");
                loaded = false;
            }
        }
    }
}
