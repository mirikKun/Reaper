using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Players
{
    public class GameInput : ITickable, IGameInput
    {


        public event Action<Vector3> OnAreaClick;
        public event Action OnExecutionClick;
        public event Action OnCircleAttackClick;
        public event Action OnReflectAttackClick;
        public event Action OnReverseAttackClick;

        private Camera _camera;

        private const string Execute = "Execute";
        private const string CircleAttack = "CircleAttack";
        private const string ReflectAttack = "ReflectAttack";
        private const string ReverseAttack = "ReverseAttack";

        private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);

   

        public void HandleExecutionButtonUp()
        {
            if (SimpleInput.GetButtonUp(Execute))
            {
                OnExecutionClick?.Invoke();
            }
        }
        public void HandleCircleAttackButtonUp()
        {
            if (SimpleInput.GetButtonUp(CircleAttack))
            {
                OnCircleAttackClick?.Invoke();
            }
        }
        public void HandleReflectAttackButtonUp()
        {
            if (SimpleInput.GetButtonUp(ReflectAttack))
            {
                OnReflectAttackClick?.Invoke();
            }
        }
        public void HandleReverseAttackButtonUp()
        {
            if (SimpleInput.GetButtonUp(ReverseAttack))
            {
                OnReverseAttackClick?.Invoke();
            }
        }
        public void Tick()
        {
            HandleScreenClick();
            HandleExecutionButtonUp();
            HandleCircleAttackButtonUp();
            HandleReflectAttackButtonUp();
            HandleReverseAttackButtonUp();
        }

        private void HandleScreenClick()
        {
            GetCamera();
            if (Input.GetButtonDown("Fire1"))
            {
                bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();

                if (Physics.Raycast(TouchRay, float.MaxValue, 1) && !isOverUI)
                {
                    Vector3 position = GetVerticalRayPosition(TouchRay);
                    OnAreaClick?.Invoke(position);
                    // playerCommandsExecutor.AddMoveToCommand(GetVerticalRayPosition(_touchRay));
                }
            }
        }

        private void GetCamera()
        {
            if (_camera == null)
            {
                _camera = Camera.main;
            }
        }

        public Vector3 GetVerticalRayPosition(Ray ray)
        {
            RaycastHit hit;
            Vector3 pos = Vector3.zero;

            if (Physics.Raycast(ray, out hit, float.MaxValue, 1))
            {
                int x = (int)(hit.point.x);
                int z = (int)(hit.point.z);
                pos = new Vector3(x, 0.2f, z);
            }

            return pos;
        }
    }
}