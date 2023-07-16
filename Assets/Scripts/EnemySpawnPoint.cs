using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private Transform model;
     private Rect _rect;

     public Rect GetSpawnRect()
     {
         Vector3 pos = transform.position;
         Vector3 scale = model.localScale;
         _rect.Set(pos.x-scale.x/2, pos.z-scale.z/2, scale.x, scale.z);
         return _rect;
     }

     public Vector3 GetSpawnPosition()
     {
         return transform.position;
     }
}
