using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Attackable : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(_damage);
        }
        
    }


}
