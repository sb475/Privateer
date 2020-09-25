﻿using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.UI;
using UnityEngine;

namespace RPG.Cinematic
{    
    public class CameraController : MonoBehaviour
    {
        public Transform target;

        public Vector3 offset;
        public float zoomSpeed = 4f;
        public float minZoom = 5f;
        public float maxZoom = 15f;
        float timeSinceClick;
        public float pitch = 2f;

        public float yawSpeed = 500f;

        private float currentZoom = 10f;
        public float currentYaw = 0f;
        

        private void Update() {
            currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);


            if (Input.GetMouseButton(1))
            {
                //reset timer
                

                timeSinceClick += Time.deltaTime;
                if (timeSinceClick > 0.2f)
                {
                    ActionMenu.HideMenuOptions_Static();
                    
                    currentYaw += Input.GetAxis("Mouse X") * yawSpeed * Time.deltaTime;
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                timeSinceClick = 0;
            }

        }

        private void LateUpdate() {
            transform.position = target.position - offset * currentZoom;
            transform.LookAt(target.position + Vector3.up * pitch);

            transform.RotateAround(target.position, Vector3.up, currentYaw);
        }

        public void SetCameraTarget(CrewMember crewMember)
        {
            target = crewMember.transform;
        }

        public void RotateCamera()
        {
            
        }
    
    }
}