using System;
using UnityEngine;
using UnityEngine.UI;

namespace Players
{
    public class GameInput : MonoBehaviour, IGameInput
    {
        [SerializeField] private Button _circleAttackButton;
        [SerializeField] private Button _reflectAttackButton;
        [SerializeField] private Button _reverseAttackButton;

        public event Action<Vector3> OnAreaClick;
        public event Action OnExecutionClick;
        public event Action OnCircleAttackClick;
        public event Action OnReflectAttackClick;
        public event Action OnReverseAttackClick;

        private Camera camera;

        private Ray _touchRay => camera.ScreenPointToRay(Input.mousePosition);

        private void Start()
        {
            camera = Camera.main;
            _circleAttackButton.onClick.AddListener(UseCircleAttack);
            _reflectAttackButton.onClick.AddListener(UseReflectAttack);
            _reverseAttackButton.onClick.AddListener(UseReverseAttack);
        }

        private void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();

                if (Physics.Raycast(_touchRay, float.MaxValue, 1) && !isOverUI)
                {
                    Vector3 position = GetVerticalRayPosition(_touchRay);
                    OnAreaClick?.Invoke(position);
                    // playerCommandsExecutor.AddMoveToCommand(GetVerticalRayPosition(_touchRay));
                }
            }

            if (Input.GetButtonDown("Fire2"))
            {
                OnExecutionClick?.Invoke();
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

        private void UseReverseAttack()
        {
            OnReverseAttackClick?.Invoke();
        }

        private void UseReflectAttack()
        {
            OnReflectAttackClick?.Invoke();
        }

        private void UseCircleAttack()
        {
            OnCircleAttackClick?.Invoke();
        }
    }
}