using System;
using UnityEngine;

namespace Players
{
    public interface IGameInput
    {
        event Action<Vector3> OnAreaClick;
        event Action OnExecutionClick;
        event Action OnCircleAttackClick;
        event Action OnReflectAttackClick;
        event Action OnReverseAttackClick;
    }
}