using System;
using UnityEngine;

public class BuildingManager : MonoBehaviour {
    
    private Camera _mainCamera;
    private BuildingTypeListSO _buildingTypeList;
    private BuildingTypeSO _buildingType;

    private void Awake() {
        _buildingTypeList = Resources.Load<BuildingTypeListSO>(nameof(BuildingTypeListSO));
        _buildingType = _buildingTypeList.list[0];
    }

    private void Start() {
        _mainCamera = Camera.main;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Instantiate(_buildingType.prefab, GetMouseWorldPosition(), Quaternion.identity);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            _buildingType = _buildingTypeList.list[0];
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            _buildingType = _buildingTypeList.list[1];
        }
    }

    private Vector3 GetMouseWorldPosition() {
        var mouseWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;

        return mouseWorldPosition;
    }
}
