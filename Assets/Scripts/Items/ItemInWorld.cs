using RPG.Base;
using RPG.Combat;
using RPG.Control;
using RPG.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.Items
{

    public class ItemInWorld : Interactable
    {
        [SerializeField] ItemConfig item = null;
        [SerializeField] int itemQuantity = 1;

        public override void Awake() {
            base.Awake();
            defaultCursorType = CursorType.Pickup;
            displayName = item.name;
            interactionPoint = gameObject.transform;
            InitializeOutline();

        }

        // private void OnTriggerEnter(Collider other)
        // {

        //         //Only do something if gameobject is the "Player". Can be updated in future to party characters.
        //        if (other.gameObject.GetComponents<CrewMember>() != null)
        //         {
        //             // //Guard statement in case there is no player click cache
        //             // if (other.GetComponent<CrewController>().GetCachePlayerClick() == null) 
        //             // {
        //             //     return;
        //             // }
        //             // //Nested if to avoid error message. 
        //             // else if (other.GetComponent<CrewController>().GetCachePlayerClick()[0] == gameObject)
        //             // {
        //             DefaultInteract(other.GetComponent<CrewController>().GetSelectedCrewMember());
                    
        //         }
        // }

        public override void DefaultInteract(ControllableObject playerController)
        {

            PickUp(playerController);
        }

        public override void PickUp(ControllableObject playerController)
        {
            (playerController as CrewMember).AddItemToInventory(item, itemQuantity);

            Destroy(gameObject);
            InfoToolTip.HideToolTip_Static();
        }

        //automate placement of things later
        // private void ShowPrefab (ItemConfig item)
        // {

        // }



        // //Function that enables respawn. Set to boolian option in the future.
        // private IEnumerator HideForSeconds(float seconds)
        // {
        //     ShowPickup(false);
        //     yield return new WaitForSeconds(seconds);
        //     ShowPickup(true);
        // }

        // private void ShowPickup(bool shouldShow) // this can be used to do character selection.
        // {
        //     GetComponent<Collider>().enabled = shouldShow;
        //     foreach (Transform child in transform)
        //     {
        //         child.gameObject.SetActive(shouldShow);
        //     }
        // }
    }

}