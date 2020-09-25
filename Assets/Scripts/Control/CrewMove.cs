using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class CrewMove : MonoBehaviour
    {
        private CrewController crewController;
        private float moveSpeed = 10f;

        // Start is called before the first frame update
        void Awake () => crewController = GetComponent<CrewController>();

        private void FixedUpdate() {
            
            float horizontal = Input.GetAxis("Horizonal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(horizontal, 0, vertical);
            Vector3 movement = transform.TransformDirection(direction) * moveSpeed;
        }
    }


}
