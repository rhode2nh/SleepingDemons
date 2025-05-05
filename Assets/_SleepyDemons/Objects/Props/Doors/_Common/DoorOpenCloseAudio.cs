using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenCloseAudio : MonoBehaviour
{
    [SerializeField] private GameObject _door;
    [SerializeField] private AudioSource _lockSound;
    [SerializeField] private AudioClip _doorOpen;
    [SerializeField] private AudioClip _doorClose;
    [SerializeField] private float _openAngleThreshold;
    [SerializeField] private float _closeAngleThreshold;
    [SerializeField] private bool _isOpen;
    private float _initialAngle;
    private bool _doorOpenSoundPlayed;
    private bool _doorClosedSoundPlayed;
    [SerializeField] private float _currentAngle;

    // Start is called before the first frame update
    void Start()
    {
        _initialAngle = _door.transform.localEulerAngles.y;
        _currentAngle = _initialAngle - 360f;
        _isOpen = false;
        _doorOpenSoundPlayed = false;
        _doorClosedSoundPlayed = true;
    }

    // Update is called once per frame
    void Update()
    {
        _currentAngle = _door.transform.localEulerAngles.y;
        if (_currentAngle > 180f) {
            _currentAngle -= 360f;
        }
        CheckDoorState();
        PlayDoorStateAudio();
    }

    void CheckDoorState() {
        if (_currentAngle + _openAngleThreshold < _initialAngle) {
            _isOpen = true;
        } else if (_currentAngle + _openAngleThreshold > _initialAngle) {
            _isOpen = false;
        }
    }

    void PlayDoorStateAudio() {
        if (_isOpen && !_doorOpenSoundPlayed) {
            // play door open sound
            _lockSound.clip = _doorOpen;
            _lockSound.Play();
            _doorOpenSoundPlayed = true;
            _doorClosedSoundPlayed = false;
        } else if (!_isOpen && !_doorClosedSoundPlayed) {
            // play door close sound
            _lockSound.clip = _doorClose;
            _lockSound.Play();
            _doorClosedSoundPlayed = true;
            _doorOpenSoundPlayed = false;
        }
    }

    public bool IsOpen() {
        return _isOpen;
    }
}
