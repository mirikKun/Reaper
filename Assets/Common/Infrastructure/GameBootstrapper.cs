using CodeBase.Logic;
using Common.Infrastructure.States;
using UnityEngine;

namespace Common.Infrastructure
{
  public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
  {
     public LoadingCurtain CurtainPrefab;
    private Game _game;

    private void Awake()
    {
      _game = new Game(this, Instantiate(CurtainPrefab));
      _game.StateMachine.Enter<BootstrapState>();

      DontDestroyOnLoad(this);
    }
  }
}