using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private int count=20;
    [SerializeField] private float areaSize = 50;
    [SerializeField] private ObstaclesFactory obstaclesFactory;

    private Pillar[] _pillars;
    private void Start()
    {
        _pillars = new Pillar[count];
        for (int i = 0; i < count; i++)
        {
            var pillar = obstaclesFactory.GetPillar();
            
        }
    }
}
