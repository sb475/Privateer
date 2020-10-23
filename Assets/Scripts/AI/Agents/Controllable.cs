using RPG.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controllable : GAgent
{
    new void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("wanderStation", 1, false);
        goals.Add(s1, 3);
    }

    public void DirectedGoal (SubGoal directedGoal)
    {
        CancelCurrentGoal();
        goals.Add(directedGoal, 100);
    }

    void NeedRelief()
    {
        beliefs.ModifyState("busting", 0);
        Invoke("NeedRelief", Random.Range(2, 5));
    }
}
