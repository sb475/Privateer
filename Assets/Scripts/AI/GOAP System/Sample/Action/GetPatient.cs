﻿//using RPG.AI;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GetPatient : GAction
//{
//    GameObject resource;
//    public override bool PrePerform()
//    {
//        target = GWorld.Instance.GetQueue("patients").RemoveResource();
//        if (target == null)
//            return false;

//        resource = GWorld.Instance.GetQueue("cubicles").RemoveResource();
//        if (resource != null)
//            inventory.AddItem(resource);
//        else
//        {
//            GWorld.Instance.GetQueue("patients").AddResource(target);
//            target = null;
//            return false;
//        }

//        GWorld.Instance.GetGOAPStates().ModifyState("FreeCubicle", -1);
//        return true;
//    }

//    public override bool PostPerform()
//    {
//        GWorld.Instance.GetGOAPStates().ModifyState("Waiting", -1);
//        if (target)
//            target.GetComponent<GAgent>().memory.AddItem(resource);
//        return true;
//    }

//    public override bool OnInterrupt()
//    {
//        return true;
//    }
//}
