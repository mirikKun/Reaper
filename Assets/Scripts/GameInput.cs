using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    [SerializeField] private CommandsExecutor _commandsExecutor;
    [SerializeField] private Camera _camera;
    private Vector3 _lastPos;
    private Ray _touchRay => _camera.ScreenPointToRay(Input.mousePosition);
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _commandsExecutor.AddMoveToCommand(GetRayPosition(_touchRay));
        }

        if (Input.GetButtonDown("Fire2"))
        {
            _commandsExecutor.StartExecuting();
        }
    }
    public Vector3 GetRayPosition(Ray ray)
    {
        RaycastHit hit;
        Vector3 pos=Vector3.zero;
        
        if (Physics.Raycast(ray, out hit,float.MaxValue,1))
        {
            int x = (int)(hit.point.x);
            int z = (int)(hit.point.z);
            pos = new Vector3(x, 0, z);
        }

        _lastPos = hit.point;
        return pos;
    }

   
}
