using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraController : MonoBehaviour
{
    [FormerlySerializedAs("commandsExecutor")] [SerializeField] private PlayerCommandsExecutor playerCommandsExecutor;
    [SerializeField] private Transform player;
    [SerializeField] private Transform destination;
    private CinemachineVirtualCamera _virtualCamera;

    private void Awake()
    {
        _virtualCamera=GetComponent<CinemachineVirtualCamera>();
    }

    private void OnEnable()
    {
        playerCommandsExecutor.OnExecutionEnd += ChangeAimTargetToDestination;
        playerCommandsExecutor.OnExecutionStart += ChangeAimTargetToPlayer;
    }
    private void OnDisable()
    {
        playerCommandsExecutor.OnExecutionEnd -= ChangeAimTargetToDestination;
        playerCommandsExecutor.OnExecutionStart -= ChangeAimTargetToPlayer;
    }

    private void ChangeAimTargetToPlayer()
    {
        _virtualCamera.Follow = player;
        _virtualCamera.LookAt = player;
    }
    private void ChangeAimTargetToDestination()
    {
        _virtualCamera.Follow = destination;
        _virtualCamera.LookAt = destination;
    }
}
