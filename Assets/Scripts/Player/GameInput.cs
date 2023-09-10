using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameInput : MonoBehaviour
{
    [SerializeField] private CommandsExecutor commandsExecutor;
    [SerializeField] private Camera camera;

    private Ray _touchRay => camera.ScreenPointToRay(Input.mousePosition);

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (Physics.Raycast(_touchRay, float.MaxValue, 1))
            {
                commandsExecutor.AddMoveToCommand(GetVerticalRayPosition(_touchRay));
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            commandsExecutor.StartExecuting();
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