
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector3 _previousPosition;
    private Vector3 _newPosition;
    public bool Moving { get; private set; }
    private float _movingProgress=3;
    private float _progressFactor=0;
    private Vector3 _yPos=new Vector3(0,1f,0);
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    public void MoveTo(Vector3 position)
    {
        _previousPosition = _transform.position;
        _newPosition = position+_yPos;
        _movingProgress = 0;
        _progressFactor = speed / Vector3.Distance(_previousPosition, _newPosition);
        Moving = true;
    }

    public Vector3 GetPos()
    {
        return _transform.position;
    }
    private void Update()
    {
        if (Moving)
        {
            _transform.position = Vector3.Lerp(_previousPosition, _newPosition,  _movingProgress);
            _movingProgress += Time.deltaTime * speed;

            if (_movingProgress >= 1)
            {            
                _movingProgress-=1;
                Moving = false;
            }
        }
    }
}
