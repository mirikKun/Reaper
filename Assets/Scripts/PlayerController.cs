using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private AnimationCurve speedCurve;
    [SerializeField] private float restTime = 0.5f;
    [SerializeField] private float maxDistance=10;
    private Vector3 _previousPosition;
    private Vector3 _newPosition;
    public PlayerState State => _state;
    public float MaxDistance => maxDistance;
    private float _stateProgress = 0;
    private float _progressFactor = 0;
    //private Vector3 _yPos = new Vector3(0, 1f, 0);
    private Transform _transform;
    private PlayerState _state=PlayerState.WaitingCommand;

    private void Awake()
    {
        _transform = transform;
        

    }

    public void MoveTo(Vector3 position)
    {
        _previousPosition = _transform.position;
        _newPosition = new Vector3(position.x,1,position.z);
        

        _stateProgress = 0;
        _progressFactor = speed / Vector3.Distance(_previousPosition, _newPosition);

        _state = PlayerState.Moving;
    }

    

    public void StartResting()
    {
        _stateProgress = 0;
        _progressFactor = 1 / restTime;
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
        _transform.position = Vector3.Lerp(_previousPosition, _newPosition, _stateProgress);
        _stateProgress += Time.deltaTime * _progressFactor * speedCurve.Evaluate(_stateProgress);

        if (_stateProgress >= 1)
        {
            _stateProgress -= 1;
            StartResting();
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
        Gizmos.DrawWireSphere(position, maxDistance);
    }
}


public enum PlayerState
{
    Moving,
    Rest,
    WaitingCommand
}