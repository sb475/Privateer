using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using RPG.AI;

public class WInterface : MonoBehaviour
{

    GameObject focusObj;
    ResourceData foData;
    GameObject newResourcePrefab;
    public GameObject[] allResources;
    Vector3 goalPos;
    public NavMeshSurface surface;
    public GameObject hospital;
    Vector3 clickOffset = Vector3.zero;
    bool offSetCalc = false;
    bool deleteResource = false;

    public void MouseOnHoverTrash()
    {
        deleteResource = true;
    }

    public void MouseOutHoverTrash()
    {
        deleteResource = false;
    }

    public void ActivateToilet()
    {
        newResourcePrefab = allResources[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out hit))
                return;

            offSetCalc = false;
            clickOffset = Vector3.zero;

            GResources r = hit.transform.gameObject.GetComponent<GResources>();

            if (r != null)
            {
                focusObj = hit.transform.gameObject;
                foData = r.info;
            }
            else if (newResourcePrefab  != null)
            {
                goalPos = hit.point;

                focusObj = Instantiate(newResourcePrefab, goalPos, newResourcePrefab.transform.rotation);
                foData = focusObj.GetComponent<GResources>().info;
            }

            if (focusObj) focusObj.GetComponent<Collider>().enabled = false;

        }
        else if(focusObj && Input.GetMouseButtonUp(0))
        {
            if (deleteResource)
            {
                GWorld.Instance.GetQueue(foData.resourceQueue).RemoveResource(focusObj);
                GWorld.Instance.GetGOAPStates().ModifyState(foData.resourceState, -1);
                Destroy(focusObj);
            }
            else
            {
                focusObj.transform.parent = hospital.transform;

                GWorld.Instance.GetQueue(foData.resourceQueue).AddResource(focusObj);
                GWorld.Instance.GetGOAPStates().ModifyState(foData.resourceState, 1);
                focusObj.GetComponent<Collider>().enabled = true;
                
            }
            surface.BuildNavMesh();
            focusObj = null;
        }    

        else if (focusObj && Input.GetMouseButton(0))
        {
            int layerMask = 1 << 8;
            RaycastHit hitMove;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out hitMove, Mathf.Infinity, layerMask))
                return;

            if(!offSetCalc)
            {
                clickOffset = hitMove.point - focusObj.transform.position;
                offSetCalc = true;
            }

            goalPos = hitMove.point - clickOffset;
            focusObj.transform.position = goalPos;
        }

        if (focusObj && Input.GetKeyDown(KeyCode.Less) || Input.GetKeyDown(KeyCode.Comma))
        {
            focusObj.transform.Rotate(0, 90, 0);
        }
        if (focusObj && Input.GetKeyDown(KeyCode.Greater) || Input.GetKeyDown(KeyCode.Period))
        {
            focusObj.transform.Rotate(0, -90, 0);
        }
    }
}
