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
        _pillars = new Pillar[count];
        for (int i = 0; i < count; i++)
        {
            Vector3 newPos = new Vector3(Random.Range(-areaSize, areaSize), 0, Random.Range(-areaSize, areaSize));
            if (newPos.InsideRects(spawnRects))
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
