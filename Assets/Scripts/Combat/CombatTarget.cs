using UnityEngine;
using RPG.Attributes;
using RPG.Control;
using RPG.Stats;
using UnityEngine.EventSystems;
using RPG.Base;
using System;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    
    public class CombatTarget : Interactable
    {

        private void Awake() {
            defaultCursorType = CursorType.Pickup;
            displayName = gameObject.name;
            InitializeOutline();
        }


        public override bool HandleRaycast(PlayerController callingController)
        {
            Fighter rayCaster = callingController.GetComponent<Fighter>();

            if (!rayCaster.CanAttack(gameObject))
            {
                return false;
            }

            if (Input.GetMouseButton(0))
            {
                rayCaster.GetComponent<Fighter>().Attack(gameObject);
            }
            if (Input.GetMouseButton(1))
            {
                Debug.Log("Right click on: " + gameObject.name);
            }

            //GetComponent<TargetDisplay>().SetTargetOverview();
            //this allows us to show that target is valid for specific action, reason returning true outside of if statement
            return true;
        }

        //RPG.Stats.Progression.ProgressionStat
        internal void Scan(PlayerController callingController)
        {
            BaseStats npcTargetStat = GetComponent<BaseStats>();
            Debug.Log (gameObject.name);

            Debug.Log (npcTargetStat.GetStat(Stat.Armor));
            Debug.Log(npcTargetStat.GetStat(Stat.Health));
            Debug.Log(npcTargetStat.GetLevel());
        
        }
    }
}