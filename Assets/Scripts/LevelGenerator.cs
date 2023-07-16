using Unity.AI.Navigation;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private int count=20;
    [SerializeField] private float areaSize = 50;
    [SerializeField] private ObstaclesFactory obstaclesFactory;
    [SerializeField] private NavMeshSurface navMeshSurface;

    private Pillar[] _pillars;
    private void Start()
    {
        //GeneratePillars();
        navMeshSurface.BuildNavMesh();
    }

    private void GeneratePillars()
    {
        _pillars = new Pillar[count];
        for (int i = 0; i < count; i++)
        {
            var pillar = obstaclesFactory.GetPillar();
            pillar.transform.position =
                new Vector3(Random.Range(-areaSize, areaSize), 0, Random.Range(-areaSize, areaSize));
            _pillars[i] = pillar;
        }
    }
}
