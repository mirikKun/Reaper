using UnityEngine;

namespace Players
{
    public class PlayerMover : MonoBehaviour
    {
         [SerializeField] private float _speed=26;
        [SerializeField] private AnimationCurve speedCurve;
         [SerializeField] private float _restTime = 0.3f;
         [SerializeField] private float _maxDistance = 15;
        private Vector3 _previousPosition;
        private Vector3 _newPosition;
        public PlayerState State => _state;
        public float MaxDistance => _maxDistance;
        private float _stateProgress = 0;
        private float _progressFactor = 0;
        private Transform _transform;
        private PlayerState _state = PlayerState.WaitingCommand;
        public bool FastMoving { get; set; }

        private void Awake()
        {
            _transform = transform;
        }

        public void MoveTo(Vector3 position)
        {
            _previousPosition = _transform.position;
            _newPosition = new Vector3(position.x, 0, position.z);


            _stateProgress = 0;
            _progressFactor = _speed / Vector3.Distance(_previousPosition, _newPosition);

            _state = PlayerState.Moving;
        }


        public void StartResting()
        {
            _stateProgress = 0;
            _progressFactor = 1 / _restTime;
            _state = PlayerState.Rest;
        }

        public Vector3 GetPos()
        {
            return _transform.position;
        }

        private void Update()
        {
            switch (_state)
            {
                case (PlayerState.Moving):
                    Move();
                    break;
                case (PlayerState.Rest):
                    Rest();
                    break;
            }
        }

        private void Move()
        {
            _stateProgress += Time.deltaTime * _progressFactor * speedCurve.Evaluate(_stateProgress);
            _transform.position = Vector3.Lerp(_previousPosition, _newPosition, _stateProgress);
            if (_stateProgress >= 1)
            {
                _stateProgress -= 1;
                if (FastMoving)
                {
                    _state = PlayerState.WaitingCommand;
                }
                else
                {
                    StartResting();
                }
            }
        }

        private void Rest()
        {
            _stateProgress += Time.deltaTime * _progressFactor;
            if (_stateProgress >= 1)
            {
                _stateProgress -= 1;
                _state = PlayerState.WaitingCommand;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector3 position = transform.position;
            position.y += 0.01f;
            Gizmos.DrawWireSphere(position, _maxDistance);
        }
    }


    public enum PlayerState
    {
        Moving,
        Rest,
        WaitingCommand
    }
}