using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour {

    [SerializeField] private Gradient gradient;
    [SerializeField] private float secondsPerDay = 10f;

    private Light2D _light2D;
    private float _dayTime;
    private float _dayTimeSpeed;
    
    private void Awake() {
        _light2D = GetComponent<Light2D>();
        _dayTimeSpeed = 1 / secondsPerDay;
    }

    private void Update() {
        _dayTime += Time.deltaTime * _dayTimeSpeed;
        _light2D.color = gradient.Evaluate(_dayTime % 1f);
    }
}
