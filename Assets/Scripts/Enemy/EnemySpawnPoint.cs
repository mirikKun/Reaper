using System;
using UnityEngine;
using Random = UnityEngine.Random;

[SelectionBase]
public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private Transform model;
    private Rect _rect;
    public Rect GetSpawnRect()
    {
        Vector3 pos = transform.position;
        Vector3 scale = model.localScale;
        _rect.Set(pos.x - scale.x / 2, pos.z - scale.z / 2, scale.x, scale.z);
        return _rect;
    }

 

    public Vector3 GetSpawnPosition()
    {
        return transform.position;
    }

    public Vector3 GetRandomSpawnPosition()
    {
   
        Vector3 offset = new Vector3(Random.Range(-_rect.width, _rect.width) / 2, 0,
            Random.Range(-_rect.height, _rect.height) / 2);

        Vector3 newPoint = transform.position + offset;

        return newPoint;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.position;
        position.y += 0.01f;
        Gizmos.DrawWireCube(position, new Vector3(_rect.width,3,_rect.height));
    }
}