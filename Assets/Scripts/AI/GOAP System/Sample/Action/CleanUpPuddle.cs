//using RPG.AI;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CleanUpPuddle : GAction
//{
//    public override bool PrePerform()
//    {
//        target = GWorld.Instance.GetQueue("puddles").RemoveResource();
//        if (target == null)
//            return false;
//        inventory.AddItem(target);
//        GWorld.Instance.GetGOAPStates().ModifyState("FreePuddle", -1);
//        return true;
//    }

//    public override bool PostPerform()
//    {
//        inventory.RemoveItem(target);
//        Destroy(target);
//        return true;
//    }

//    public override bool OnInterrupt()
//    {
//        return true;
//    }
//}
