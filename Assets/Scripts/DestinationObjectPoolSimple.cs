using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Simplest possible object pool
public class DestinationObjectPoolSimple : MonoBehaviour
{
    [SerializeField] private Destination bulletPrefab;

    private int _initialPoolSize = 3;

    private int _maxPoolSize = 10;

    //The bullet prefab we instantiate

    //Store the pooled bullets here
    private readonly List<Destination> _destinations = new();
    private List<Destination> _activeDestination=new List<Destination>();


    private void Start()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("Need a reference to the destination prefab");
        }

        //Instantiate new bullets and put them in a list for later use
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

    //Generate a single new bullet and put it in list
    private void GenerateDestination()
    {
        Destination newDestination = Instantiate(bulletPrefab, transform);

        newDestination.gameObject.SetActive(false);

        _destinations.Add(newDestination);
    }


    //Get a bullet from the pool
    public Destination GetDestination()
    {
        //Try to find an inactive bullet
        foreach (Destination destination in _destinations)
        {
            if (!destination.gameObject.activeInHierarchy)
            {
                destination.gameObject.SetActive(true);

                _activeDestination.Add(destination);
                return destination;
            }
        }

        //We are out of bullets so we have to instantiate another bullet (if we can)
        if (_destinations.Count < _maxPoolSize)
        {
            GenerateDestination();

            //The new bullet is last in the list so get it
            Destination lastDestination = _destinations[^1];

            lastDestination.gameObject.SetActive(true);
            _activeDestination.Add(lastDestination);

            return lastDestination;
        }

        return null;
    }
}