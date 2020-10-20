//using RPG.AI;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GetTreated : GAction
//{
//    public override bool PrePerform()
//    {
//        Debug.Log("Starting GetTreated");
//        target = localMemory.FindItemWithTag("Cubicle");
//        if (target == null)
//            return false;
//        return true;
//    }

//    public override bool PostPerform()
//    {
//        Debug.Log("Finished GetTreated");
//        GWorld.Instance.GetGOAPStates().ModifyState("Treated", 1);
//        beliefs.ModifyState("isCured", 1);
//        localMemory.RemoveItem(target);
//        return true;
//    }

//    public override bool OnInterrupt()
//    {
//        return true;
//    }
//}
