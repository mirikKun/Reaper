using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class CircleAttack : MonoBehaviour
{
    [SerializeField] private float _preAttackTime = 0.3f;
    [SerializeField] private float _attackTime = 0.5f;
    [SerializeField] private float _postAttackTime = 0.3f;
    [SerializeField] private Transform _attackEffect;
    [SerializeField] private float _effectSpeed = 10;

    [SerializeField] private int _damage = 1;
    [SerializeField] private float _radius = 6;
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
        yield return new WaitForSeconds(_preAttackTime);

        _attackEffect.gameObject.SetActive(true);
        _attackEffect.localScale = Vector3.one * (_radius * 2);
        DoDamage();
        while (_progress < _attackTime)
        {
            _attackEffect.Rotate(Vector3.up, _attackEffect.transform.eulerAngles.y + Time.deltaTime * _effectSpeed);
            _progress += Time.deltaTime;
            yield return null;
        }

        _attackEffect.gameObject.SetActive(false);

        yield return new WaitForSeconds(_postAttackTime);
        AttackEnded = true;
    }

    private void DoDamage()
    {
        Collider[] buffer = new Collider[100];
        int bufferedCount = Physics.OverlapSphereNonAlloc(transform.position, _radius, buffer, EnemyLayerMask);
        for (int i = 0; i < bufferedCount; i++)
        {
            buffer[i].GetComponent<IDamageable>().TakeDamage(_damage);
        }
    }
}