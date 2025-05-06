using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoorCreakController : MonoBehaviour
{
    [SerializeField] private List<float> _volumeList;
    [SerializeField] private float _currentNormalizedVelocity;
    [SerializeField] private float pitchAmount;
    [SerializeField] private float initialPitch;
    
    private Door _door;
    private List<DoorCreak> _doorCreakList;

    void Awake()
    {
        _door = GetComponentInParent<Door>();
        _doorCreakList = GetComponentsInChildren<DoorCreak>().ToList();
    }
    
    void Start() {
        for (int i = 0; i < _doorCreakList.Count; i++) {
            _volumeList.Add(0.0f);
        }
    }

    void Update() {
        _currentNormalizedVelocity = _door.GetNormalizedVelocity();

        for (int i = 0; i < _doorCreakList.Count; i++) {
            _volumeList[i] = Mathf.Lerp(0.0f, _doorCreakList[i].End, _currentNormalizedVelocity);
            _volumeList[i] = (_volumeList[i] - _doorCreakList[i].Start) / (_doorCreakList[i].End - _doorCreakList[i].Start);
            _doorCreakList[i].CreakAudio.volume = _doorCreakList[i].volumeCurve.Evaluate(_volumeList[i]);
            _doorCreakList[i].CreakAudio.pitch = initialPitch + (_currentNormalizedVelocity * pitchAmount);

            if (_doorCreakList[i].CreakAudio.volume == 0.0f && _doorCreakList[i].CreakAudio.isPlaying)
            {
                _doorCreakList[i].CreakAudio.Stop();
            }

            else if (_doorCreakList[i].CreakAudio.volume > 0.0f && !_doorCreakList[i].CreakAudio.isPlaying)
            {
                _doorCreakList[i].CreakAudio.Play();
            }
        }
    }
}
