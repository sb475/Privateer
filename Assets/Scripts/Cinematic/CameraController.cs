using System.Collections;
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
        public float zoomSpeed = 50f;
        public float minZoom = 5f;
        public float maxZoom = 25f;
        public float pitch = 2f;
        public float yawSpeed = .1f;
        public float tiltSpeed = .1f;
        public float cameraDistance = 10f;

        float timeSinceClick;
        float lastFrameTime;

        [SerializeField] private float currentZoom = 10f;
        public float currentYaw = 0f;
        public float currentTilt = 0f;
        private void Start()
        {
            lastFrameTime = Time.realtimeSinceStartup;
        }

        public float GetCameraZoom ()
        {
            return currentZoom;
        }

        public void SetCameraMaxZoom(float value)
        {   
            maxZoom = value;
        }
        public void SetZoomSpeed(float value)
        {
            zoomSpeed = value;
        }

        

        private void LateUpdate() {

            float cameraTime = Time.realtimeSinceStartup - lastFrameTime;

            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
                currentZoom *= (cameraDistance * 0.3f);
            }    
                
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
            if (Input.GetMouseButton(1))
            {
                //reset timer
                timeSinceClick += cameraTime;
                if (timeSinceClick > 0.2f)
                {
                    ActionMenu.HideMenuOptions_Static();

                    currentYaw += Input.GetAxis("Mouse X") * yawSpeed * cameraTime;
                    currentTilt -= Input.GetAxis("Mouse Y") * tiltSpeed * cameraTime;

                    //this is a very useful function, need to remember it
                    currentTilt = Mathf.Clamp(currentTilt, 0f, 90f);
                }
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                timeSinceClick = 0;
            }


            transform.position = target.position - offset * currentZoom;
            transform.LookAt(target.position + Vector3.up * pitch);
            transform.RotateAround(target.position, Vector3.up, currentYaw);
            transform.RotateAround(target.position, Vector3.right, currentTilt);
        }

        public void SetCameraTarget(IControllable controllable)
        {
            target = controllable.gameObject.transform;
        }
    
    }
}
