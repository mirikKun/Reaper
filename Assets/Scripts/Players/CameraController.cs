using Cinemachine;
using Commands;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Players
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineTargetGroup _targetGroup;
        [SerializeField] private float _executionCameraDamping;
        [SerializeField] private float _preparingCameraDamping;
        private PlayerCommandsExecutor _playerCommandsExecutor;
        private CinemachineTransposer _virtualCameraBody;
        private const float PlayerWeight = 1.68f;

        [Inject]
        private void Construct(PlayerCommandsExecutor playerCommandsExecutor)
        {
            _playerCommandsExecutor = playerCommandsExecutor;
            _targetGroup.AddMember(_playerCommandsExecutor.transform, PlayerWeight, 0);
        }

        private void Awake()
        {
            _virtualCameraBody = GetComponent<CinemachineVirtualCamera>()
                .GetCinemachineComponent<CinemachineTransposer>();
        }

        private void Start()
        {
            ChangeCameraSpeedToPreparing();
        }

        private void OnEnable()
        {
            _playerCommandsExecutor.OnCommandsReady += ChangeCameraSpeedToPreparing;
            _playerCommandsExecutor.OnExecutionStart += ChangeCameraSpeedToExecution;
            _playerCommandsExecutor.OnExecutionEnd += ClearTargetGroup;
            _playerCommandsExecutor.OnMoveCommandAdding += AddToTargetGroup;
        }

        private void OnDisable()
        {
            _playerCommandsExecutor.OnCommandsReady -= ChangeCameraSpeedToPreparing;
            _playerCommandsExecutor.OnExecutionStart -= ChangeCameraSpeedToExecution;
            _playerCommandsExecutor.OnExecutionEnd -= ClearTargetGroup;
            _playerCommandsExecutor.OnMoveCommandAdding -= AddToTargetGroup;

        }

        private void AddToTargetGroup(Transform destination)
        {
            _targetGroup.AddMember(destination,1,0);
        }
        private void ClearTargetGroup()
        {
            if (_targetGroup.m_Targets.Length < 1)
                return;
            _targetGroup.m_Targets = new[] { _targetGroup.m_Targets[0] };
        }

        private void ChangeCameraSpeedToExecution()
        {
            _virtualCameraBody.m_XDamping = _executionCameraDamping;
            _virtualCameraBody.m_YDamping = _executionCameraDamping;
            _virtualCameraBody.m_ZDamping = _executionCameraDamping;
        }

        private void ChangeCameraSpeedToPreparing()
        {
            _virtualCameraBody.m_XDamping = _preparingCameraDamping;
            _virtualCameraBody.m_YDamping = _preparingCameraDamping;
            _virtualCameraBody.m_ZDamping = _preparingCameraDamping;
        }
    }
}