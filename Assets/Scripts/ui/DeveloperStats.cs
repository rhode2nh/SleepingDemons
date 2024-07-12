using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeveloperStats : MonoBehaviour
{
    [SerializeField] private TMP_Text fps;

    [SerializeField] private float _updateRate;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(_updateRate);
            fps.SetText("fps: " + (1.0f / Time.smoothDeltaTime).ToString("000.0"));
        }
    }
}
