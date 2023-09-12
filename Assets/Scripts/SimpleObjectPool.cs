using System;
using System.Collections.Generic;
using UnityEngine;


public class SimpleObjectPool<T>  where T : MonoBehaviour
{
    public event Action OnNewElementNeed;
     private T elementPrefab ;

    private int _initialPoolSize = 3;

    private int _maxPoolSize = 10;


    private readonly List<T> _elements = new();
    private List<T> _activeElements=new();



    public void SetupPool(int maxCount)
    {
        _maxPoolSize = maxCount;
        for (int i = 0; i < _initialPoolSize; i++)
        {
            OnNewElementNeed?.Invoke();
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

    public void AddElement(T newElements)
    {
        
        _elements.Add(newElements);
    }
    


    public T GetElements()
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
            OnNewElementNeed?.Invoke();

            T lastDestination = _elements[^1];

            lastDestination.gameObject.SetActive(true);
            _activeElements.Add(lastDestination);

            return lastDestination;
        }

        return null;
    }
}