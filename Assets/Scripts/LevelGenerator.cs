using Factories;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private int _count=45;
    [SerializeField] private float _areaSize = 50;
    [SerializeField] private ObstaclesFactory obstaclesFactory;
    [SerializeField] private NavMeshSurface navMeshSurface;

    private Pillar[] _pillars;
    
    public void LevelGeneratorSetup( Rect[] spawnRects)
    {
        ClearLevel();
        GeneratePillars(spawnRects);
        navMeshSurface.BuildNavMesh();
    }
    private void ClearLevel()
    {
        if (_pillars == null)
        {
            return;
        }
        foreach (var pillar in _pillars)
        {
            pillar.Recycle();
        }
    }
    

    private void GeneratePillars( Rect[] spawnRects)
    {
        Random.InitState(1);
        _pillars = new Pillar[_count];
        for (int i = 0; i < _count; i++)
        {
            Vector3 newPos = new Vector3(Random.Range(-_areaSize, _areaSize), 0, Random.Range(-_areaSize, _areaSize));
            if (newPos.InsideRects(spawnRects,obstaclesFactory.MaxScale/2))
            {
                i--;
                continue;
            }
            var pillar = obstaclesFactory.GetPillar();
            pillar.transform.position = newPos;
            _pillars[i] = pillar;
        }
    }
}
