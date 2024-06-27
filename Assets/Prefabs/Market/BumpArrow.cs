using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpArrow : MonoBehaviour
{
    [SerializeField] private GameObject _arrow;
    [SerializeField] private float _bumpSpeed;
    [SerializeField] private float _maxSize;
    [SerializeField] private float _totalTime;
    [SerializeField] private float _preferredSize;

    private float _lerpedSize;
    private float _timePassed = 0.0f;
    private Vector3 initialSize;
    // Start is called before the first frame update
    void Start()
    {
        initialSize = _arrow.transform.localScale;   
        _arrow.transform.localScale = new Vector3(_preferredSize, _preferredSize, _preferredSize);
    }

    void Update()
    {
        _timePassed += Time.deltaTime;
    }

    public void Bump()
    {
        StopAllCoroutines();
        ResetArrowScale();
        StartCoroutine(BumpAnimation()); 
    }

    private void ResetArrowScale()
    {
        _arrow.transform.localScale = initialSize;
        _lerpedSize = 0.0f;
        _timePassed = 0.0f;
    }

    IEnumerator BumpAnimation()
    {
        while(_timePassed < _totalTime)
        {
            _lerpedSize = Mathf.Lerp(_arrow.transform.localScale.x, _maxSize, Time.deltaTime * _bumpSpeed);
            _arrow.transform.localScale = new Vector3(_lerpedSize, _lerpedSize, _lerpedSize);
            yield return new WaitForEndOfFrame();
        }
    }
}
