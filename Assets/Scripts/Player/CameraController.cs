using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private CommandsExecutor commandsExecutor;
    [SerializeField] private Transform player;
    [SerializeField] private Transform destination;
    private CinemachineVirtualCamera _virtualCamera;

    private void Awake()
    {
        _virtualCamera=GetComponent<CinemachineVirtualCamera>();
    }

    private void OnEnable()
    {
        commandsExecutor.OnExecutionEnd += ChangeAimTargetToDestination;
        commandsExecutor.OnExecutionStart += ChangeAimTargetToPlayer;
    }
    private void OnDisable()
    {
        commandsExecutor.OnExecutionEnd -= ChangeAimTargetToDestination;
        commandsExecutor.OnExecutionStart -= ChangeAimTargetToPlayer;
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
