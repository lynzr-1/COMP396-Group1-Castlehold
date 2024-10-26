using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCameraZoom : MonoBehaviour
{
    public float zoomSpeed = 200;
    public float zoomSmoothness = 5;

    public float minZoom = 2;
    public float maxZoom = 15;

    private float _currentZoom = 8;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        _currentZoom = Mathf.Clamp(_currentZoom - Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime, minZoom, maxZoom);
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _currentZoom, zoomSmoothness * Time.deltaTime);
    }
}
