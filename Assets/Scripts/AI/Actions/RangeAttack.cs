using RPG.AI;
using RPG.Attributes;
using RPG.Combat;
using RPG.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.AI
{

    public class RangeAttack: GAction
    {
        public RangeAttack()
        {
            actionName = "Range Attack";
            customDestination = true;
        }

        public override bool PrePerform()
        {
            
            target = GWorld.Instance.GetList("hostiles").GetResource(0);
            if (target == null) return false;

            //if the current weapon does not have projectiles, look to see if secondary does, if not cannot range attack.
            if (!agent.fighter.currentWeaponConfig.HasProjectile())
            {
                if ((agent.fighter.equipment.secondaryWeapon as WeaponConfig).HasProjectile())
                {
                    if (agent.fighter.currentWeaponConfig == agent.fighter.equipment.secondaryWeapon) return false;
                    agent.fighter.EquipWeapon(agent.fighter.equipment.LoadWeapon(agent.fighter.equipment.secondaryWeapon));
                }
                else
                {
                    return false;
                }
            }

            float weaponRange = agent.fighter.GetWeaponRange();
            if (Vector3.Distance(target.transform.position, this.transform.position) < weaponRange)
            {
                actionDestination = this.transform.position;
                return true;
            }
            else
            {
                Vector3 targetDir = this.transform.position - target.transform.position;
                Vector3 inRangeForAttackPos = target.transform.position + targetDir.normalized * weaponRange;
                actionDestination = inRangeForAttackPos;
                return true;
            }  
        }

        public override bool PerformAction()
        {
            agent.fighter.Attack(target.GetComponent<IDamagable>());
            return true;
        }

        public override bool PostPerform()
        {
            //Debug.Log(this + " PostPerform");

            beliefs.RemoveState("threatened");
            //beliefs.RemoveState("readyToEngage");
            target = null;
            timesInterrupted = 0;
            ResetAction();
            return true;
        }

        public override bool OnInterrupt()
        {
            timesInterrupted++;
            ResetAction();
            return true;
        }

        public override void ActionComplete()
        {
            if (!actionComplete)
            {
                //Debug.Log("Action completed successfully");
                //agent.CompleteAction();
                actionComplete = true;
            }


        }
    }


}
