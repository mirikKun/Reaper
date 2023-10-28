using UnityEngine;

namespace UI
{
    public class Mediator:MonoBehaviour
    {
        [SerializeField] private Game.Game _game;
        [SerializeField] private UIGameStateSwitch _gameStateSwitch;
        [SerializeField] private LoseScreen _loseScreen;


        public void SwitchFocusState() => _game.SwitchFocusState();
        public void OpenLoseScreen() => _loseScreen.TurnOnLoseScreen();
        public void CloseLoseScreen() => _loseScreen.TurnOffLoseScreen();

        public void TurnOnFocusBackground() => _gameStateSwitch.TurnOnFocusBackground();
        public void TurnOffFocusBackground() => _gameStateSwitch.TurnOffFocusBackground();

    }
}