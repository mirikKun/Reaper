using UnityEngine;

namespace UI
{
    public class LoseScreen: MonoBehaviour
    {
        [SerializeField] private GameObject _loseScreen;

        public void TurnOnLoseScreen()
        {
            _loseScreen.SetActive(true);
        }

        public void TurnOffLoseScreen()
        {
            _loseScreen.SetActive(false);
        }
    }
}