using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] _virtualCamera;
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
        SwitchCam();
    }
    private void Update()
    {
        if(_myInputs.Player.Zoom.triggered)
        {
            SwitchCam();
        }
    }
    public void SwitchCam()
    {
        foreach (var cam in _virtualCamera)
        {
            cam.Priority = -1;
        }
        _virtualCamera[_currentCameraIndex].Priority = 10;

        _currentCameraIndex++;
        _currentCameraIndex %= _virtualCamera.Length;
    }
}
