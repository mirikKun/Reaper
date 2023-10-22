using System;
using Commands;
using UnityEngine;

namespace Players
{
    public class GameInput : MonoBehaviour
    {
        [SerializeField] private PlayerCommandsExecutor playerCommandsExecutor;
         private Camera camera;

        private Ray _touchRay => camera.ScreenPointToRay(Input.mousePosition);

        private void Start()
        {
            camera=Camera.main;
        }

        private void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();

                if (Physics.Raycast(_touchRay, float.MaxValue, 1)&&!isOverUI)
                {
                    playerCommandsExecutor.AddMoveToCommand(GetVerticalRayPosition(_touchRay));
                }
            }

            if (Input.GetButtonDown("Fire2"))
            {
                playerCommandsExecutor.StartExecuting();
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