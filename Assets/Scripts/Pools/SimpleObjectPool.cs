using System.Collections.Generic;
using UnityEngine;

namespace Pools
{
    public class SimpleObjectPool<T>:MonoBehaviour  where T : MonoBehaviour
    {
        [SerializeField] private T _prefab;

        private T _elementPrefab ;

        private int _initialPoolSize = 3;

        private int _maxPoolSize = 10;


        private readonly List<T> _elements = new();
        private List<T> _activeElements=new();

        public List<T> ActiveElements => _activeElements;

        private void GenerateNewElement()
        {
            T newElements = Instantiate(_prefab, transform);
            newElements.gameObject.SetActive(false);
            _elements.Add(newElements);
        }

        public void SetupPool(int maxCount)
        {   if (_prefab == null)
            {
                Debug.LogError("Need a reference to the destination prefab");
            }
            _maxPoolSize = maxCount;
            for (int i = 0; i < _initialPoolSize; i++)
            {
                GenerateNewElement();
            }
        }

        public void RevertAllToPool()
        {
            foreach (var element in _activeElements)
            {
                element.gameObject.SetActive(false);
            }
            _activeElements.Clear();
        }

        public T GetElement()
        {
            foreach (T destination in _elements)
            {
                if (!destination.gameObject.activeInHierarchy)
                {
                    destination.gameObject.SetActive(true);

                    _activeElements.Add(destination);
                    return destination;
                }
            }

            if (_elements.Count < _maxPoolSize)
            {
                GenerateNewElement();
                T lastDestination = _elements[^1];

                lastDestination.gameObject.SetActive(true);
                _activeElements.Add(lastDestination);

                return lastDestination;
            }

            return null;
        }
    }
}