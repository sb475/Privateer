using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    protected Transform _XForm_Camera;
    protected Transform focusPoint;

    public Transform target;

    protected Vector3 _LocalRotation;
    protected float currentZoom = 10f;
    public float minZoom = 5f;
    public float maxZoom = 25f;
    public float minTiltAngle = 45f;
    public float maxTitleAngle = 75f;
    public float offset = 2f;
    public float MouseSensitivity = 4f;
    public float ScrollSensitvity = 2f;
    public float OrbitDampening = 10f;
    public float ScrollDampening = 6f;

    public bool CameraDisabled = false;

    float timeSinceClick;
    float lastFrameTime;


    // Use this for initialization
    void Start()
    {
        this._XForm_Camera = this.transform;
        this.focusPoint = this.transform.parent;
        lastFrameTime = Time.realtimeSinceStartup;

    }
    public void SetCameraTarget(IControllable controllable)
    {
        target = controllable.gameObject.transform;
    }

    public float GetCameraZoom()
    {
        return currentZoom;
    }

    public void SetCameraMaxZoom(float value)
    {
        maxZoom = value;
    }
    public void SetZoomSpeed(float value)
    {
        ScrollDampening = value;
    }


    void LateUpdate()
    {
        float cameraTime = Time.realtimeSinceStartup - lastFrameTime;
        focusPoint.position = new Vector3(target.position.x, target.position.y + offset, target.position.z);

        if (!CameraDisabled)
        {
            if (Input.GetMouseButton(1))
            {
                timeSinceClick += cameraTime;
                if (timeSinceClick > 0.2f)
                {
                    //Rotation of the Camera based on Mouse Coordinates
                    if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                    {
                        _LocalRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
                        _LocalRotation.y += Input.GetAxis("Mouse Y") * MouseSensitivity;

                        //Clamp the y Rotation to horizon and not flipping over at the top
                        _LocalRotation.y = Mathf.Clamp(_LocalRotation.y, minTiltAngle, maxTitleAngle);

                    }
                    //Zooming Input from our Mouse Scroll Wheel
                    if (Input.GetAxis("Mouse ScrollWheel") != 0f)
                    {
                        float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitvity;

                        ScrollAmount *= (this.currentZoom * 0.3f);

                        this.currentZoom += ScrollAmount * -1f;

                        this.currentZoom = Mathf.Clamp(this.currentZoom, minZoom, maxZoom);
                    }
                }
            }
        }

        //Actual Camera Rig Transformations
        Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
        this.focusPoint.rotation = Quaternion.Lerp(this.focusPoint.rotation, QT, cameraTime * OrbitDampening);

        if (this._XForm_Camera.localPosition.z != this.currentZoom * -1f)
        {
            this._XForm_Camera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this._XForm_Camera.localPosition.z, this.currentZoom * -1f, cameraTime * ScrollDampening));
        }

        if (Input.GetMouseButtonUp(1))
        {
            timeSinceClick = 0;
        }
    }
}
