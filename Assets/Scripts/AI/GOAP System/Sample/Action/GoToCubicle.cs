﻿//using RPG.AI;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GoToCubicle : GAction
//{
//    public override bool PrePerform()
//    {
//        target = inventory.FindItemWithTag("Cubicle");
//        if (target != null) return true;
//        return false;
//    }

//    public override bool PostPerform()
//    {
//        GWorld.Instance.GetGOAPStates().ModifyState("TreatingPatient", 1);
//        GWorld.Instance.GetQueue("cubicles").AddResource(target);
//        inventory.RemoveItem(target);
//        GWorld.Instance.GetGOAPStates().ModifyState("FreeCubicle", 1);
//        return true;
//    }

//    public override bool OnInterrupt()
//    {
//        return true;
//    }
//}