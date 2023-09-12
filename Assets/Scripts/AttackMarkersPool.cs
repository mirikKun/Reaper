using UnityEngine;
    public class AttackMarkersPool:MonoBehaviour
    {
        [SerializeField] private AttackMarker prefab;
        public SimpleObjectPool<AttackMarker> Pool;

        public void SetupPool(int maxCount)
        {
            if (prefab == null)
            {
                Debug.LogError("Need a reference to the destination prefab");
            }
            Pool.SetupPool(maxCount);
        }
        private void OnEnable()
        {
            Pool = new SimpleObjectPool<AttackMarker>();
            Pool.OnNewElementNeed += InstantiateNewDestination;
        }
        private void OnDisable()
        {
            Pool.OnNewElementNeed -= InstantiateNewDestination;
        }

        private void InstantiateNewDestination()
        {
            AttackMarker newElements = Instantiate(prefab, transform);
            newElements.gameObject.SetActive(false);
            Pool.AddElement(newElements);
        }
    }
