using System;
using Cinemachine;
using UnityEngine;

public class CameraHandler : MonoBehaviour {
    
    public static CameraHandler Instance { get; private set; }
    
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private float _orthographicSize;
    private float _targetOrthographicSize;
    private bool _edgeScrolling;

    private void Awake() {
        Instance = this;

        _edgeScrolling = PlayerPrefs.GetInt("edgeScrolling", 1) == 1;
    }

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

        if (_edgeScrolling) {
            const float edgeScrollingSize = 40f;
            if (Input.mousePosition.x > Screen.width - edgeScrollingSize) {
                x = +1f;
            }
            if (Input.mousePosition.x < edgeScrollingSize) {
                x = -1f;
            }
            if (Input.mousePosition.y > Screen.height - edgeScrollingSize) {
                y = +1f;
            }
            if (Input.mousePosition.y < edgeScrollingSize) {
                y = -1f;
            }
        }

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

    public void SetEdgeScrolling(bool edgeScrolling) {
        _edgeScrolling = edgeScrolling;
        PlayerPrefs.SetInt("edgeScrolling", edgeScrolling?1:0);
    }

    public bool GetEdgeScrolling() {
        return _edgeScrolling;
    }
}
