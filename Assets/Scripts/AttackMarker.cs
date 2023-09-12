using UnityEngine;

public class AttackMarker : MonoBehaviour
{
    [SerializeField] private float height=0.2f;
    

    public void SetPosition(Vector3 pos)
    {
        transform.position = new Vector3(pos.x, height, pos.z);
    }
}
