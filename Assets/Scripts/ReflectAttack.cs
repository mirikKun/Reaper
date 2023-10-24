using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class ReflectAttack : MonoBehaviour
{
    [SerializeField] private float _maxWaitingTime = 3f;
    [SerializeField] private float _attackSpeed = 5f;
    [SerializeField] private float _postAttackTime = 0.3f;
    [SerializeField] private Transform _attackEffect;
    [SerializeField] private float _checkRadius = 2;


    [SerializeField] private int _damage = 1;
    [SerializeField] private float _attackRadius = 3;
    [SerializeField] private float _attackLenght = 5;
    private float _minLenght = 0.01f;
    private const int EnemyLayerMask = 1 << 10;

    private float _progress;
    public bool AttackEnded { get; private set; }

    public void BeginAttack()
    {
        _progress = 0;
        AttackEnded = false;
        StartCoroutine(Attacking());
    }

    private IEnumerator Attacking()
    {
        _progress = 0;


        Vector3 direction = Vector3.zero;
        Collider[] buffer = new Collider[15];

        Vector3 checkPoint = transform.position;
        while (_progress < _maxWaitingTime)
        {
            if (Physics.OverlapSphereNonAlloc(checkPoint, _checkRadius, buffer, EnemyLayerMask) > 0)
            {
                direction = buffer[0].transform.position - checkPoint;
                _progress = _maxWaitingTime;
            }

            _progress += Time.deltaTime;
            yield return null;
        }


        _progress = 0;
        if (direction != Vector3.zero)
        {
            _attackEffect.gameObject.SetActive(true);
            float angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
            _attackEffect.rotation = Quaternion.Euler(0, angle, 0);
            _attackEffect.localScale = new Vector3(_attackEffect.localScale.x, _attackEffect.localScale.y, _minLenght);
            while (_progress < _attackLenght)
            {
                _progress += Time.deltaTime * _attackSpeed;
                _attackEffect.localScale = new Vector3(_attackEffect.localScale.x, _attackEffect.localScale.y, _progress);
                _attackEffect.position = checkPoint + direction * _progress / 2 + Vector3.up;
                yield return null;
            }

            DoDamage(direction);

            _attackEffect.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(_postAttackTime);
        AttackEnded = true;
    }

    private void DoDamage(Vector3 direction)
    {
        Collider[] buffer = new Collider[100];
        Vector3 startPoint = transform.position;
        int bufferedCount = Physics.OverlapCapsuleNonAlloc(startPoint, startPoint + direction * _attackLenght,
            _attackRadius, buffer, EnemyLayerMask);
        for (int i = 0; i < bufferedCount; i++)
        {
            buffer[i].GetComponent<IDamageable>().TakeDamage(_damage);
        }
    }
}