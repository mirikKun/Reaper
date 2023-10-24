using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIGameStateSwitch : MonoBehaviour
{
   [SerializeField] private Image _focusBackground;
   [SerializeField] private GameObject _loseScreen;

   public void TurnOnLoseScreen()
   {
      _loseScreen.SetActive(true);
   }

   public void TurnOffLoseScreen()
   {
      _loseScreen.SetActive(false);
   }
   
   public void TurnOnFocusBackground()
   {
      _focusBackground.enabled = true;
   }

   public void TurnOffFocusBackground()
   {
      _focusBackground.enabled = false;
   }
}
