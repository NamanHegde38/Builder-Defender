using System;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour {
    
    public static CinemachineShake Instance { get; private set; }
    
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;
    private float _timer;
    private float _timerMax;
    private float _startingIntensity;
    
    private void Awake() {
        Instance = this;
        
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cinemachineBasicMultiChannelPerlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update() {
        if (_timer < _timerMax) {
            _timer += Time.deltaTime;
            var amplitude = Mathf.Lerp(_startingIntensity, 0f, _timer / _timerMax);
            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = amplitude;
        }
    }

    public void ShakeCamera(float intensity, float timerMax) {
        _timerMax = timerMax;
        _startingIntensity = intensity;
        _timer = 0f;
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
    }
}
