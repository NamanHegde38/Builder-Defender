using System;
using Cinemachine;
using UnityEngine;

public class CameraHandler : MonoBehaviour {

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private float _orthographicSize;
    private float _targetOrthographicSize;

    private void Start() {
        _orthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        _targetOrthographicSize = _orthographicSize;
    }

    private void Update() {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement() {
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");

        var moveDir = new Vector3(x, y).normalized;
        const float moveSpeed = 25f;
        transform.position += moveDir * (moveSpeed * Time.deltaTime);
    }
    
    private void HandleZoom() {
        const float zoomAmount = 1.5f;
        _targetOrthographicSize += -Input.mouseScrollDelta.y * zoomAmount;
        
        const float minOrthographicSize = 10f;
        const float maxOrthographicSize = 30f;
        
        _targetOrthographicSize = Mathf.Clamp(_targetOrthographicSize, minOrthographicSize, maxOrthographicSize);

        const float zoomSpeed = 5f;
        _orthographicSize = Mathf.Lerp(_orthographicSize, _targetOrthographicSize, Time.deltaTime * zoomSpeed);
        
        cinemachineVirtualCamera.m_Lens.OrthographicSize = _orthographicSize;
    }
}
