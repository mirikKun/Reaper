using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    [SerializeField] private CommandsExecutor _commandsExecutor;
    private const int OBSTACLE_LAYER_MASK = 1 << 9;
    [SerializeField] private Camera _camera;
    private float _backSpace = 1;
    private Ray _touchRay => _camera.ScreenPointToRay(Input.mousePosition);
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (Physics.Raycast(_touchRay,float.MaxValue,1))
            {
                _commandsExecutor.AddMoveToCommand(GetVerticalRayPosition(_touchRay));
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            _commandsExecutor.StartExecuting();
        }
    }
    public Vector3 GetVerticalRayPosition(Ray ray)
    {
        RaycastHit hit;
        Vector3 pos=Vector3.zero;
        
        if (Physics.Raycast(ray, out hit,float.MaxValue,1))
        {
            int x = (int)(hit.point.x);
            int z = (int)(hit.point.z);
            pos = new Vector3(x, 0.2f, z);
        }

        pos = GetHorizontalPosition(new Ray(_commandsExecutor.LastPos, pos - _commandsExecutor.LastPos), pos);
        return pos;
    }

    public Vector3 GetHorizontalPosition(Ray ray,Vector3 firstPos)
    {
        RaycastHit hit;
        Vector3 pos=firstPos;
        if (Physics.Raycast(ray, out hit,(ray.origin-pos).magnitude,OBSTACLE_LAYER_MASK))
        {
            pos = hit.point;
        }

        return pos;
    }

   
}
