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
        StartCoroutine(WaitForTransition(SetPlayerInBed));
    }

    private void GetOutOfBed()
    {
        _fpsController.ResetPovCamera();
        _activeCamera.Priority = _fpsCam.Priority - 1;
        _activeCamera = _fpsCam;
        StartCoroutine(WaitForTransition(SetPlayerInBed));
    }

    private IEnumerator WaitForTransition(Action onComplete)
    {
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
        else
        {
            if (_currentCoroutine == null) return;
            StopCoroutine(_currentCoroutine);
            _vignette.ResetValues();
        }
    }

    private IEnumerator InitializeSleepState()
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
            _vignette.SetIntensity(currentIntensity);
            yield return new WaitForEndOfFrame();
        }

        yield return StartCoroutine(TryToFallAsleep());
    }

    private IEnumerator TryToFallAsleep()
    {
        float startIntensity = 1.0f;
        float currentIntensity = startIntensity;
        float endIntensity = 0.0f;
        float elapsedTime = 0.0f;
        float duration = 1.0f;
        Vector2 initialLook = _input.Look;
        float resetThreshold = 0.1f;
        
        while (currentIntensity > endIntensity)
        {
            if (Vector2.Distance(initialLook, _input.Look) > resetThreshold)
            {
                elapsedTime = 0.0f;
                initialLook = _input.Look;
            }
            float t = elapsedTime / duration;
            elapsedTime += Time.deltaTime;
            currentIntensity = Mathf.Lerp(startIntensity, endIntensity, t);
            _vignette.SetSmoothness(currentIntensity);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(2.0f);
        SleepManager.Instance.Sleep();

        yield return StartCoroutine(WakeUp());
    }

    private IEnumerator WakeUp()
    {
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
            _vignette.SetSmoothness(currentIntensity);
            yield return new WaitForEndOfFrame();
        }
    }
}
