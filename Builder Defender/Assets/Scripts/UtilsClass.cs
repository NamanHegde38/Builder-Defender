using UnityEngine;
using Debug = System.Diagnostics.Debug;

public static class UtilsClass {

    private static Camera _mainCamera;
    
    public static Vector3 GetMouseWorldPosition() {
        if (_mainCamera == null) _mainCamera = Camera.main;

        Debug.Assert(_mainCamera != null, nameof(_mainCamera) + " != null");
        
        var mouseWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;

        return mouseWorldPosition;
    }

    public static Vector3 GetRandomDir() {
        return new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
            ).normalized;
    }

    public static float GetAngleFromVector(Vector3 vector) {
        var radians = Mathf.Atan2(vector.y, vector.x);
        var degrees = radians * Mathf.Rad2Deg;
        return degrees;
    }
}
