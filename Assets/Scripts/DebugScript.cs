using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject availableCrew;
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("D was pressed");
            availableCrew.transform.parent = this.transform;
        }
    }
}
