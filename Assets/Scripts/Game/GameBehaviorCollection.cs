using System.Collections.Generic;

namespace Game
{
    public class EnemyBehaviorCollection 
    {
        private List<GameBehavior> _behaviors = new List<GameBehavior>();

        public bool IsEmpty => _behaviors.Count == 0;
        public void Add(GameBehavior behavior)
        {
            _behaviors.Add(behavior);

        }

        public void StopNavMeshMoving()
        {
            foreach (var behavior in _behaviors)
            {
                behavior.Stop();
            }
        }

        public void ContinueNavMeshMoving()
        {
            foreach (var behavior in _behaviors)
            {
 
                behavior.Continue();
            }
        }
        public void GameUpdate()
        {
            for (int i = 0; i < _behaviors.Count; i++)
            {
                if (!_behaviors[i].GameUpdate())
                {
                    int lastIndex = _behaviors.Count - 1;
                    _behaviors[i] = _behaviors[lastIndex];
                    _behaviors.RemoveAt(lastIndex);
                    i -= 1;
                }
            }
        }

        public void Clear()
        {
            for (int i = 0; i < _behaviors.Count; i++)
            {
                _behaviors[i].Recycle();
            }
            _behaviors.Clear();
        }
    }
}
