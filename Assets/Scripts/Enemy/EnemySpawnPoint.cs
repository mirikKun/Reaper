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
        _rect.Set(pos.x - scale.x / 2, pos.z - scale.z / 2, scale.x, scale.z);
        Debug.Log(_rect.width);
        return _rect;
    }

    public Vector3 GetSpawnPosition()
    {
        return transform.position;
    }

    public Vector3 GetRandomSpawnPosition()
    {
        return transform.position + new Vector3(Random.Range(-_rect.width / 2, _rect.width / 2), 0,
            Random.Range(-_rect.height / 2, _rect.height / 2));
    }
}