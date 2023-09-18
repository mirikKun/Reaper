using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraController : MonoBehaviour
{
     [SerializeField] private PlayerCommandsExecutor playerCommandsExecutor;
     [SerializeField] private float executionCameraDamping;
     [SerializeField] private float preparingCameraDamping;
    private CinemachineTransposer _virtualCameraBody;
    

    private void Awake()
    {
        _virtualCameraBody=GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Start()
    {
        ChangeCameraSpeedToPreparing();
    }

    private void OnEnable()
    {
        playerCommandsExecutor.OnCommandsReady += ChangeCameraSpeedToPreparing;
        playerCommandsExecutor.OnExecutionStart += ChangeCameraSpeedToExecution;
    }
    private void OnDisable()
    {
        playerCommandsExecutor.OnCommandsReady -= ChangeCameraSpeedToPreparing;
        playerCommandsExecutor.OnExecutionStart -= ChangeCameraSpeedToExecution;
    }

    private void ChangeCameraSpeedToExecution()
    {
        _virtualCameraBody.m_XDamping = executionCameraDamping;
        _virtualCameraBody.m_YDamping = executionCameraDamping;
        _virtualCameraBody.m_ZDamping = executionCameraDamping;
    }
    private void ChangeCameraSpeedToPreparing()
    {
        _virtualCameraBody.m_XDamping = preparingCameraDamping;
        _virtualCameraBody.m_YDamping = preparingCameraDamping;
        _virtualCameraBody.m_ZDamping = preparingCameraDamping;
    }

}
