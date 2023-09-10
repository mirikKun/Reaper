using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameStateSwitch : MonoBehaviour
{
   [SerializeField] private Image focusBackground;
   [SerializeField] private Image[] actionPoints;
   [SerializeField] private Image actionPointsBar;
   [SerializeField] private GameObject loseScreen;

   [SerializeField] private Color activePointsColor;
   [SerializeField] private Color spentPointsColor;

   private int _actionPointCount= 5;


   public void ChangeActionPointCount(int newCount)
   {
      _actionPointCount = Mathf.Clamp(newCount, 1, actionPoints.Length );
      for (var i = 0; i < actionPoints.Length; i++)
      {
         var point = actionPoints[i];
         point.gameObject.SetActive(_actionPointCount > i);
         if (_actionPointCount > i)
         {
            //TODO
         }
      }
   }
   public void FillActionPoint(int leftPoint)
   {
      FillActionPoints((float)leftPoint / _actionPointCount);
   }
   public void FillActionPoints(float value)
   {
      actionPointsBar.fillAmount = value;
      for (int i = 0; i < _actionPointCount; i++)
      {
         if (value >= (i + 1f) / _actionPointCount)
         {
            actionPoints[i].color = activePointsColor;
         }
         else
         {
            actionPoints[i].color = spentPointsColor;

         }
      }
   }
   
   
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
