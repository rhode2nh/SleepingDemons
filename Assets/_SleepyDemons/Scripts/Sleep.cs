using System;
using System.Collections;
using Cinemachine;
using StarterAssets;
using UnityEngine;

public class Sleep : MonoBehaviour
{
    [SerializeField] private float _intensityScalar;
    
    private CinemachineVirtualCamera _fpsCam;
    private CinemachineBrain _brain;
    private CinemachineVirtualCamera _activeCamera;
    private FirstPersonController _fpsController;
    private StarterAssetsInputs _input;
    private VignetteEffect _vignette;

    private Coroutine _currentCoroutine;
    private Coroutine _inBedCoroutine;
    
    // Sleep values
    private float _currentSleepiness;
    private float _elapsedTime;
    private int _sign = 1;

    private void Awake()
    {
        _fpsCam = GetComponentInChildren<CinemachineVirtualCamera>();
        _fpsController = GetComponent<FirstPersonController>();
        _brain = GetComponentInChildren<CinemachineBrain>();
        _vignette = GetComponentInChildren<VignetteEffect>();
        _activeCamera = _fpsCam;
    }

    private void Start()
    {
        _input = InputManager.Instance.GetComponent<StarterAssetsInputs>();
        _input.OnGetOutOfBed += GetOutOfBed;
    }

    public void GetInBed(POVCamera povCamera)
    {
        _activeCamera = povCamera.VirtualCamera;
        _activeCamera.Priority = _fpsCam.Priority + 1;
        _fpsController.SetActivePovCamera(povCamera);
        _inBedCoroutine = StartCoroutine(WaitForTransition(null, SetPlayerInBed));
    }

    private void GetOutOfBed()
    {
        Debug.Log("Getting out of bed");
        _fpsController.ResetPovCamera();
        _activeCamera.Priority = _fpsCam.Priority - 1;
        _activeCamera = _fpsCam; 
        StartCoroutine(WaitForTransition(ResetValues, SetPlayerInBed));
    }

    private IEnumerator WaitForTransition(Action onStart, Action onComplete)
    {
        onStart?.Invoke();
        yield return new WaitForEndOfFrame();
        _input.SetInputActive(false);
        PlayerManager.Instance.InTransitionState = true;
        yield return new WaitUntil(() => !_brain.IsBlending);
        PlayerManager.Instance.InTransitionState = false;
        _input.SetInputActive(true);
        onComplete?.Invoke();
    }

    private void SetPlayerInBed()
    {
        PlayerManager.Instance.IsInBed = !PlayerManager.Instance.IsInBed;

        if (PlayerManager.Instance.IsInBed)
        {
            _currentCoroutine = StartCoroutine(InitializeSleepState());
        }
    }

    private void ResetValues()
    {
        if (_currentCoroutine == null) return;
        StopCoroutine(_currentCoroutine);
        StopCoroutine(_inBedCoroutine);
        _vignette.ResetValues();
        _elapsedTime = 0.0f;
        _currentSleepiness = 0.0f;
    }

    private IEnumerator InitializeSleepState()
    {
        Debug.Log("Initializing Sleep  STate");
        float startIntensity = 0.0f;
        float currentIntensity = startIntensity;
        float endIntensity = 1.0f;
        float elapsedTime = 0.0f;
        float duration = 1.0f;
        while (currentIntensity < endIntensity)
        {
            float t = elapsedTime / duration;
            elapsedTime += Time.deltaTime;
            currentIntensity = Mathf.Lerp(startIntensity, endIntensity, t);
            _vignette.SetIntensity(currentIntensity);
            yield return new WaitForEndOfFrame();
        }

        yield return TryToFallAsleep();
    }

    private IEnumerator TryToFallAsleep()
    {
        Debug.Log("Trying to fall asleep");
        float startIntensity = 0.0f;
        float endIntensity = 1.0f;
        float duration = 3.0f;
        float resetThreshold = 0.1f;
        
        while (_currentSleepiness < endIntensity)
        {
            if (InputManager.Instance.LookVelocity > resetThreshold)
            {
                yield return OpenEyes(_currentSleepiness, startIntensity, endIntensity, InputManager.Instance.LookVelocity * _intensityScalar, duration, _elapsedTime);
            }
            float t = _elapsedTime / duration;
            _elapsedTime += Time.deltaTime;
            _currentSleepiness = Mathf.Lerp(startIntensity, endIntensity, t);
            _vignette.SetSharpness(_currentSleepiness);
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Sleeping");
        yield return new WaitForSeconds(2.0f);
        SleepManager.Instance.Sleep();

        yield return WakeUp();
    }

    private IEnumerator OpenEyes(float threshold, float startIntensity, float endIntensity, float intensityScalar, float duration, float initalElapsedTime)
    {
        float current = endIntensity;
        while (current > startIntensity + threshold)
        {
            float t = _elapsedTime / duration;
            _elapsedTime -= Time.deltaTime * intensityScalar;
            current = Mathf.Lerp(startIntensity, endIntensity, t);
            _vignette.SetSharpness(current);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator WakeUp()
    {
        float startIntensity = 1.0f;
        float currentIntensity = startIntensity;
        float endIntensity = 0.0f;
        float elapsedTime = 0.0f;
        float duration = 1.0f;
        
        while (currentIntensity > endIntensity)
        {
            float t = elapsedTime / duration;
            elapsedTime += Time.deltaTime;
            currentIntensity = Mathf.Lerp(startIntensity, endIntensity, t);
            _vignette.SetSharpness(currentIntensity);
            yield return new WaitForEndOfFrame();
        }

        _elapsedTime = 0.0f;
    }
}
