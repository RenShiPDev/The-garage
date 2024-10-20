using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private int _startFOV;
    [SerializeField] private int _zoomFOV;
    [SerializeField] private float _FOVSpeed;

    private void Update()
    {
        if(Input.GetKey(KeyCode.Z))
        {
            if(_camera.fieldOfView > _zoomFOV)
            {
                _camera.fieldOfView -= Time.deltaTime * _FOVSpeed;
            }
            else
            {
                _camera.fieldOfView = _zoomFOV;
            }
        }
        else
        {
            if (_camera.fieldOfView < _startFOV)
            {
                _camera.fieldOfView += Time.deltaTime * _FOVSpeed;
            }
            else
            {
                _camera.fieldOfView = _startFOV;
            }
        }
    }
}
