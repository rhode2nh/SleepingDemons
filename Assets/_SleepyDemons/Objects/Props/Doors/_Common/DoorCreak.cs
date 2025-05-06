using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DoorCreak : MonoBehaviour
{
    public AnimationCurve volumeCurve;
    [field: SerializeField, Range(0.0f, 1.0f)] public float Start { get; private set; }
    [field: SerializeField, Range(0.0f, 1.0f)] public float End { get; private set; }
    public AudioSource CreakAudio { get; private set; }

    private void Awake()
    {
        CreakAudio = GetComponent<AudioSource>();
    }
}
