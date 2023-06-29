using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SwitchCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] _virtualCamera;
    [SerializeField] float _maxCameraSize = 20.0f;
    float _minCameraSize;
    int _currentCameraIndex = 0;
    MyInputs _myInputs;
    private void Awake()
    {
        _myInputs = new MyInputs();
        _myInputs.Enable();
    }
    private void OnDestroy()
    {
        _myInputs?.Dispose();
    }
    private void Start()
    {
        if (_virtualCamera[0]) _minCameraSize = _virtualCamera[0].m_Lens.OrthographicSize;
    }
    private void Update()
    {
        CameraSize(_myInputs.Player.CameraSize.ReadValue<float>());
    }
    //public void SwitchCam()
    //{
    //    foreach (var cam in _virtualCamera)
    //    {
    //        cam.Priority = -1;
    //    }
    //    _virtualCamera[_currentCameraIndex].Priority = 10;

    //    _currentCameraIndex++;
    //    _currentCameraIndex %= _virtualCamera.Length;
    //}
    public void CameraSize(float input)
    {
        float cameraSize = _virtualCamera[0].m_Lens.OrthographicSize;
        cameraSize += input;
        if (cameraSize < _minCameraSize) return;
        if (cameraSize > _maxCameraSize) return;
        _virtualCamera[0].m_Lens.OrthographicSize = cameraSize;
    }
}
