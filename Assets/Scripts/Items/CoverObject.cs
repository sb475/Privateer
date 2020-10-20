using RPG.Base;
using RPG.Control;
using UnityEngine;

namespace RPG.Items
{
    public class CoverObject : Interactable
    {

        public bool inUse;
        public GameObject objectInCover;


        public override void DefaultInteract(ControllableObject callingController)
        {
            UseCover (callingController.gameObject);
        }

        private void Update() {
            if (objectInCover != null)
            {
                if (Vector3.Distance(transform.position, objectInCover.transform.position) > interactRadius+1)
                {
                    StopUsingCover();
                }

            }
        }

        public void UseCover (GameObject objectInCover)
        {
            if (inUse || this.objectInCover == objectInCover) return;

            if (Vector3.Distance(transform.position, objectInCover.transform.position) < interactRadius + 1)
            {
                this.objectInCover = objectInCover;
                inUse = true;
            }



        }

        public void StopUsingCover ()
        {
            Debug.Log(gameObject.name + " no longer in use");
            inUse = false;
            objectInCover = null;
        }

        
    }
}