
namespace Infrastructure.States
{
  public class BootstrapState : IState
  {
    private const string Boot = "Boot";
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;

    public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader)
    {
      _stateMachine = stateMachine;
      _sceneLoader = sceneLoader;

    }

    public void Enter()
    {
      _sceneLoader.Load(Boot, onLoaded: EnterLoadLevel);
    }

    public void Exit()
    {
    }

 
    private void EnterLoadLevel() =>
      _stateMachine.Enter<LoadProgressState>();

    // private static IInputService InputService()
    // {
    //   return new StandaloneInputService();
    // }
  }
}