using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class Destination : MonoBehaviour
    {

        [SerializeField] private LineRenderer line;
        public void PlaceDestination(Vector3 from,Vector3 to)
        {
            transform.position = to;
            line.SetPosition(0,new Vector3(from.x,0.2f,from.z));
            line.SetPosition(1,new Vector3(to.x,0.2f,to.z));
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }

