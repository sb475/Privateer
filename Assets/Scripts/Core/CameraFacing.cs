using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{

    public class CameraFacing : MonoBehaviour
    {


        // Update is called once per frame
        void Update()
        {
            //set the facing direction
            transform.forward = Camera.main.transform.forward;
            {
                
            }
        }
    }
}
