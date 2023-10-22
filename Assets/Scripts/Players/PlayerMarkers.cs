using Pools;
using UnityEngine;

namespace Players
{
    public class PlayerMarkers : MonoBehaviour
    {
        public DestinationsPool DestinationsPool;
        public AttackMarkersPool CircleAttackMarkersPool;
        public AttackMarkersPool ReflectMarkersPool;
        public AttackMarker ReverseAttackMarkersPool;
    }
}