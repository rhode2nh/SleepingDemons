using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoorCreakController : MonoBehaviour
{
    [SerializeField] private List<float> _volumeList;
    [SerializeField] private PhysicsDoor _physicsDoor;
    [SerializeField] private float _currentVelocity;
    [SerializeField] private float _currentNormalizedVelocity;
    private List<DoorCreak> _doorCreakList;

    void Start() {
        _doorCreakList = GetComponentsInChildren<DoorCreak>().ToList();
        for (int i = 0; i < _doorCreakList.Count; i++) {
            _volumeList.Add(0.0f);
        }
    }

    void Update() {
        _currentVelocity = _physicsDoor.GetVelocity();
        _currentNormalizedVelocity = _physicsDoor.GetNormalizedVelocity();

        for (int i = 0; i < _doorCreakList.Count; i++) {
            _volumeList[i] = Mathf.Lerp(0.0f, _doorCreakList[i].end, _currentNormalizedVelocity);
            // return (_rb.velocity.magnitude - 0.0f) / (_maxVelocity - 0);
            _volumeList[i] = (_volumeList[i] - _doorCreakList[i].start) / (_doorCreakList[i].end - _doorCreakList[i].start);
            if (_doorCreakList[i].creakAudio != null) {
                _doorCreakList[i].creakAudio.volume = _doorCreakList[i].volumeCurve.Evaluate(_volumeList[i]);
            }
        }
    }
}
