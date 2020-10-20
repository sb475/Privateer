using RPG.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Citizen : GAgent
{
    public float statusCheck = 0.5f;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("wanderStation", 1, false);
        goals.Add(s1, 1);

        SubGoal s2 = new SubGoal("stayAlive", 1, false);
        goals.Add(s2, 2);

        //SubGoal s3 = new SubGoal("killEnemy", 1, false);
        //goals.Add(s3, 5);

       // hostile = GWorld.Instance.GetList("hostiles").GetResource(0);

        //Invoke(nameof(LookForHostiles), statusCheck);

    }

    IEnumerator LookForHostiles(float checkTime)
    {
        foreach (GAgent ag in room.GetAgentsInRoom())
            if (ag.isHostile)
                hostileAgents.Add(ag);

        foreach (GAgent hostile in hostileAgents)
        {
            if (Vector3.Distance(hostile.transform.position, this.transform.position) < 15)
            {
                if (Vector3.Distance(hostile.transform.position, this.transform.position) < 10)
                {
                    beliefs.ModifyState("tooClose", 1);
                }
                beliefs.ModifyState("threatened", 1);
                CancelCurrentGoal();
            }

        }

        yield return new WaitForSeconds(checkTime);

        StartCoroutine(LookForHostiles(checkTime));
    }


}
