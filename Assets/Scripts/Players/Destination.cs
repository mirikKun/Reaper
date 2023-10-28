using UnityEngine;

namespace Players
{
    public class Destination : MonoBehaviour
    {
   
        [SerializeField] private LineRenderer _line;

        public void PlaceDestination(Vector3 from, Vector3 to)
        {
            transform.position = to;
            _line.SetPosition(0, new Vector3(from.x, 0.2f, from.z));
            _line.SetPosition(1, new Vector3(to.x, 0.2f, to.z));
        }

    }
}