using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAttack : MonoBehaviour
{
    [SerializeField] private float preAttackTime = 0.3f;
    [SerializeField] private float attackTime = 0.5f;
    [SerializeField] private float postAttackTime = 0.3f;
    [SerializeField] private Transform attackEffect;
    [SerializeField] private float effectSpeed=10;

    [SerializeField] private int damage=1;
    [SerializeField] private float radius=5;
    private const int ENEMY_LAYER_MASK = 1 << 10;

    private float _progress;
    public bool AttackEnded { get;private set; }

    public void BeginAttack()
    {
        _progress = 0;
        AttackEnded = false;
        StartCoroutine(Attacking());
    }

    private IEnumerator Attacking()
    {
        yield return new WaitForSeconds(preAttackTime);

        attackEffect.gameObject.SetActive(true);
        attackEffect.localScale = Vector3.one * (radius * 2);
        DoDamage();
        while (_progress<attackTime)
        {
            attackEffect.Rotate(Vector3.up,attackEffect.transform.eulerAngles.y+Time.deltaTime*effectSpeed);
            _progress += Time.deltaTime;
            yield return null;
        }
        attackEffect.gameObject.SetActive(false);

        yield return new WaitForSeconds(postAttackTime);
        AttackEnded = true;
    }

    private void DoDamage()
    {
        Collider[] buffer = new Collider[100];
        int bufferedCount = Physics.OverlapSphereNonAlloc(transform.position, radius, buffer, ENEMY_LAYER_MASK);
        for (int i = 0; i < bufferedCount; i++)
        {
            buffer[i].GetComponent<IDamageable>().TakeDamage(damage);
        }
    }
}
