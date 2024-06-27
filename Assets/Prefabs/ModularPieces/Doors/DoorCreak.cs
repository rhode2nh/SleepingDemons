using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCreak : MonoBehaviour
{
    public AnimationCurve volumeCurve;
    [Range(0.0f, 1.0f)] public float start;
    [Range(0.0f, 1.0f)] public float end;
    public AudioSource creakAudio;
    
}
