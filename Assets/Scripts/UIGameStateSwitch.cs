using UnityEngine;
using UnityEngine.UI;

public class UIGameStateSwitch : MonoBehaviour
{
   [SerializeField] private Image focusBackground;
   [SerializeField] private GameObject loseScreen;

   public void TurnOnLoseScreen()
   {
      loseScreen.SetActive(true);
   }

   public void TurnOffLoseScreen()
   {
      loseScreen.SetActive(false);
   }
   
   public void TurnOnFocusBackground()
   {
      focusBackground.enabled = true;
   }

   public void TurnOffFocusBackground()
   {
      focusBackground.enabled = false;
   }
}
