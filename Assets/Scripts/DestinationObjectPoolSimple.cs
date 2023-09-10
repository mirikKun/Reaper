using System.Collections.Generic;
using UnityEngine;


public class DestinationObjectPoolSimple : MonoBehaviour
{
    [SerializeField] private Destination bulletPrefab;

    private int _initialPoolSize = 3;

    private int _maxPoolSize = 10;


    private readonly List<Destination> _destinations = new();
    private List<Destination> _activeDestination=new();


    public void SetupPool(int maxCount)
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("Need a reference to the destination prefab");
        }

        _maxPoolSize = maxCount;
        for (int i = 0; i < _initialPoolSize; i++)
        {
            GenerateDestination();
        }
    }

    public void RevertAllToPool()
    {
        foreach (var destination in _activeDestination)
        {
            destination.Hide();
        }
        _activeDestination.Clear();
    }

    private void GenerateDestination()
    {
        Destination newDestination = Instantiate(bulletPrefab, transform);

        newDestination.gameObject.SetActive(false);

        _destinations.Add(newDestination);
    }


    public Destination GetDestination()
    {
        foreach (Destination destination in _destinations)
        {
            if (!destination.gameObject.activeInHierarchy)
            {
                destination.gameObject.SetActive(true);

                _activeDestination.Add(destination);
                return destination;
            }
        }

        if (_destinations.Count < _maxPoolSize)
        {
            GenerateDestination();

            Destination lastDestination = _destinations[^1];

            lastDestination.gameObject.SetActive(true);
            _activeDestination.Add(lastDestination);

            return lastDestination;
        }

        return null;
    }
}