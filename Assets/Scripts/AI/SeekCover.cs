using System;
using System.Collections.Generic;
using RPG.Global;
using RPG.Items;
using RPG.Movement;
using UnityEngine;

namespace RPG.AI
{
    public class SeekCover : MonoBehaviour 
    {

        public Transform target;
        public CoverObject chosenCover;
        public float weaponRange;
        CharacterEngine mover;

        private void Awake() {
            mover = GetComponent<CharacterEngine>();
        }

        private void Update() {
            if (InCloseCombatRange()) return;
        

            if (isInRange()) return;
            Debug.DrawRay(transform.position, target.position);
        }

        private bool InCloseCombatRange()
        {
            if (Vector3.Distance(transform.position, target.position) < 10)
            {
                mover.MoveTo(target.position, 1f);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool isInRange()
        {
            if (Vector3.Distance(transform.position, target.position) > weaponRange)
            {
                mover.MoveTo(target.position, 1f);
                return false;
            }
            else{

                TakeCover();
                return true;

            }
        }

        void TakeCover()
        {
            float dist = Mathf.Infinity;
            Vector3 chosenSpot = Vector3.zero;

            if (WorldController.instance.coverObjects == null) return;
                
                
                for (int i = 0; i < WorldController.instance.coverObjects.Length; i++)
                {
                    Vector3 hideDir = WorldController.instance.GetCoverSpots()[i].transform.position - target.transform.position;
                    Vector3 hidePos = WorldController.instance.GetCoverSpots()[i].transform.position + hideDir.normalized * 1;

                    float distanceToCover = Vector3.Distance(this.transform.position, hidePos);
                    float coverDistanceToTarget = Vector3.Distance(hidePos, target.position);
                    
                    if (distanceToCover < dist && coverDistanceToTarget < weaponRange)
                    {
                        chosenSpot = hidePos;
                        dist = Vector3.Distance(this.transform.position, hidePos);
                        chosenCover = WorldController.instance.GetCoverSpots()[i];
                    }
                }

            if (Vector3.Distance(transform.position, chosenCover.transform.position) < chosenCover.interactRadius + 1)
            {
                Debug.Log("At Cover");
                chosenCover.UseCover(gameObject);
            }
            else
            {
                mover.MoveTo(chosenSpot, 1f);
            }



        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, weaponRange);
            Gizmos.DrawWireSphere(transform.position, 15);


        }
    }
}