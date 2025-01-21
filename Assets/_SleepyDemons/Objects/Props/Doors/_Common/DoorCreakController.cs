using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class DoorCreakController : MonoBehaviour
{
    [SerializeField] private List<float> _volumeList;
    [SerializeField] private PhysicsDoor _physicsDoor;
    [SerializeField] private float _currentNormalizedVelocity;
    [SerializeField] private List<DoorCreak> doorCreakList;
    [SerializeField] private float pitchAmount;
    [SerializeField] private float initialPitch;

    void Start() {
        // _doorCreakList = GetComponentsInChildren<DoorCreak>().ToList();
        for (int i = 0; i < doorCreakList.Count; i++) {
            _volumeList.Add(0.0f);
        }
    }

    void Update() {
        _currentNormalizedVelocity = _physicsDoor.GetNormalizedVelocity();

        for (int i = 0; i < doorCreakList.Count; i++) {
            _volumeList[i] = Mathf.Lerp(0.0f, doorCreakList[i].end, _currentNormalizedVelocity);
            // return (_rb.velocity.magnitude - 0.0f) / (_maxVelocity - 0);
            _volumeList[i] = (_volumeList[i] - doorCreakList[i].start) / (doorCreakList[i].end - doorCreakList[i].start);
            if (doorCreakList[i].creakAudio != null) {
                doorCreakList[i].creakAudio.volume = doorCreakList[i].volumeCurve.Evaluate(_volumeList[i]);
                doorCreakList[i].creakAudio.pitch = initialPitch + (_currentNormalizedVelocity * pitchAmount);
            }
        }
    }
}
