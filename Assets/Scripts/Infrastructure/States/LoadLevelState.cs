using Curtain;

namespace Infrastructure.States
{
  public class LoadLevelState : IPayloadedState<string>
  {

    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;


    public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain)
    {
      _stateMachine = gameStateMachine;
      _sceneLoader = sceneLoader;
      _loadingCurtain = loadingCurtain;

    }

    public void Enter(string sceneName)
    {
      _loadingCurtain.Show();
      _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit() =>
      _loadingCurtain.Hide();

    private void OnLoaded()
    {
      InformProgressReaders();
      _stateMachine.Enter<GameLoopState>();
    }

    private void InformProgressReaders()
    {
     
    }

    // private void InitGameWorld()
    // {
    //
    //   CameraFollow(hero);
    // }
    //
    // private void CameraFollow(GameObject hero) =>
    //   Camera.main.GetComponent<CameraFollow>().Follow(hero);
  }
}