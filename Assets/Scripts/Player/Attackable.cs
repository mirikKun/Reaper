using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Attackable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(1);
        }
        
    }


}
