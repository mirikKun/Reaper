using UnityEngine;

namespace Attack
{
    public class AttackMarker : MonoBehaviour
    {
        [SerializeField] private float _height=0.35f;
    

        public void SetPosition(Vector3 pos)
        {
            transform.position = new Vector3(pos.x, _height, pos.z);
        }
    }
}
