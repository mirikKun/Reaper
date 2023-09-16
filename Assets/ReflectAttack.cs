using System.Collections;
using UnityEngine;

public class ReflectAttack : MonoBehaviour
{
    [SerializeField] private float maxWaitingTime = 0.3f;
    [SerializeField] private float attackSpeed = 0.5f;
    [SerializeField] private float postAttackTime = 0.3f;
    [SerializeField] private Transform attackEffect;
    [SerializeField] private float checkRadius;


    [SerializeField] private int damage = 1;
    [SerializeField] private float attackRadius = 5;
    [SerializeField] private float attackLenght = 5;
    private float _minLenght = 0.01f;
    private const int ENEMY_LAYER_MASK = 1 << 10;

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
        while (_progress < maxWaitingTime)
        {
            if (Physics.OverlapSphereNonAlloc(checkPoint, checkRadius ,buffer,ENEMY_LAYER_MASK)>0)
            {
                Debug.Log("ZZZZZZZZZZZZZZZZZZZZz");
                direction = buffer[0].transform.position - checkPoint;
                _progress=maxWaitingTime;
            }

            _progress += Time.deltaTime;
            yield return null;
        }


        _progress = 0;
        if (direction != Vector3.zero)
        {
            attackEffect.gameObject.SetActive(true);
            float angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
            attackEffect.rotation=Quaternion.Euler(0,angle,0);
            attackEffect.localScale = new Vector3(attackEffect.localScale.x,attackEffect.localScale.y,_minLenght);
            while (_progress < attackLenght)
            {
                _progress += Time.deltaTime * attackSpeed;
                attackEffect.localScale = new Vector3(attackEffect.localScale.x,attackEffect.localScale.y,_progress);
                attackEffect.position = checkPoint + direction * _progress / 2+Vector3.up;
                yield return null;
            }

            DoDamage(direction);

            attackEffect.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(postAttackTime);
        AttackEnded = true;
    }

    private void DoDamage(Vector3 direction)
    {
        Collider[] buffer = new Collider[100];
        Vector3 startPoint = transform.position;
        int bufferedCount = Physics.OverlapCapsuleNonAlloc(startPoint, startPoint + direction * attackLenght,
            attackRadius, buffer, ENEMY_LAYER_MASK);
        for (int i = 0; i < bufferedCount; i++)
        {
            buffer[i].GetComponent<IDamageable>().TakeDamage(damage);
        }
    }
}