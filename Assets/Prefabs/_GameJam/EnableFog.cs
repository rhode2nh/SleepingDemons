using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableFog : MonoBehaviour
{
    public float maxFogDensity;
    public AudioSource basementAmbience;
    private Vector3 pointA;
    private Vector3 pointB;
    public float curVolume;
    private float curFog;
    private float xOffset;
    private BoxCollider boxCollider;
    public float maxVolume;

    void Start() {
        boxCollider = GetComponent<BoxCollider>();
        xOffset = boxCollider.size.x / 2.0f;
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            pointA = transform.position;
            pointA.x += xOffset;
            pointB = transform.position;
            pointB.x -= xOffset;
            if (curVolume == 0.0f) {
                curVolume = pointA.x;
                curFog = pointA.x;
                basementAmbience.Play();
                RenderSettings.fog = true;
                Debug.Log("Entering");
            }
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Player") {
            curVolume = Mathf.Clamp((other.transform.position.x - pointA.x) / (pointB.x - pointA.x), 0, maxVolume);
            curFog = Mathf.Clamp((other.transform.position.x - pointA.x) / (pointB.x - pointA.x), 0, 1.0f);
            basementAmbience.volume = curVolume;
            RenderSettings.fogDensity = maxFogDensity * curFog;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            if (curVolume == 0.0f) {
                basementAmbience.Stop();
                RenderSettings.fog = false;
            }
        }
    }
}