using System;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

public class UIGameStateSwitch : MonoBehaviour
{
   [SerializeField] private Image _focusBackground;
   [SerializeField] private Button _switchButton;
   private Mediator _mediator;

   [Inject]
   private void Construct(Mediator mediator)
   {
      _mediator = mediator;
   }
   private void Start()
   {
      _switchButton.onClick.AddListener(_mediator.SwitchFocusState);
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