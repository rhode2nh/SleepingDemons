using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class part3Text : MonoBehaviour
{
    [SerializeField]  Animator[] clips;

    [SerializeField] private float _delay;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayAnimations());
    }

    IEnumerator PlayAnimations()
    {
        for (int i = 0; i < clips.Length; i++)
        {
            yield return new WaitForSeconds(_delay);
            clips[i].Play("p");
        }
    }
}
