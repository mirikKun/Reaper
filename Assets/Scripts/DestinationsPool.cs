
    using System;
    using UnityEngine;

    public class DestinationsPool:MonoBehaviour
    {
        [SerializeField] private Destination destinationPrefab;
        public SimpleObjectPool<Destination> Pool;

        public void SetupPool(int maxCount)
        {
            if (destinationPrefab == null)
            {
                Debug.LogError("Need a reference to the destination prefab");
            }
            Pool.SetupPool(maxCount);

        }

        private void OnEnable()
        {
            Pool = new SimpleObjectPool<Destination>();
            Pool.OnNewElementNeed += InstantiateNewDestination;
        }

        private void OnDisable()
        {
            Pool.OnNewElementNeed -= InstantiateNewDestination;
        }

        private void InstantiateNewDestination()
        {
            Destination newElements = Instantiate(destinationPrefab, transform);
            newElements.gameObject.SetActive(false);
            Pool.AddElement(newElements);
        }
    }
