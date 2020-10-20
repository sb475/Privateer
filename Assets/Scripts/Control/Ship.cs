using RPG.Items;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class Ship : ControllableObject
    {
        public GameObject target;

        public override void Awake() {
            base.Awake();


            //WHat can a ship do?

            //Move towards
                //Engine
            //Dock
            //Scan
            //Fire At
                //Fire missiles
                //Fire rail gun
                //Fire machine
                //Fire anti missiles
            //Crewcomforts
            
        }


        public override void Update() {
            base.Update();
            if (target != null)
            mov.StartMoveAction(target.transform.position, 1f);
        }

        public void KeepAtRange (float range)
        {
            float rangeToTarget = Vector3.Distance(target.transform.position, transform.position);
            if (rangeToTarget < range || rangeToTarget > range)
            {
                
            }
        }
        
    }
}