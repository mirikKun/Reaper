using UnityEngine;

namespace Game
{
    [SelectionBase]

    public abstract class GameBehavior : MonoBehaviour
    {
        public virtual bool GameUpdate() => true;
        public abstract void Stop();
        public abstract void Continue();
        public abstract void Recycle();
    }
}
