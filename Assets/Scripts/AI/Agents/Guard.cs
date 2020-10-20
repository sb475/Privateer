using RPG.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Guard : GAgent
{
    public float statusCheck = 2f;
    public List<GameObject> hostiles = new List<GameObject>(); 
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("patrol", 1, false);
        goals.Add(s1, 1);

        //SubGoal s2 = new SubGoal("stayAlive", 1, false);
        //goals.Add(s2, 2);

        SubGoal s3 = new SubGoal("protectStation", 1, false);
        goals.Add(s3, 5);

      //hostile = GWorld.Instance.GetList("hostiles").GetResource(0);

       Invoke("LookForHostiles", statusCheck);

    }

    void LookForHostiles()
    {
        foreach (GAgent ag in room.GetAgentsInRoom())
            if (ag.isHostile)
               hostileAgents.Add(ag);

        foreach (GAgent hostile in hostileAgents)
        {
            if (Vector3.Distance(hostile.transform.position, this.transform.position) < 15)
            {
                beliefs.ModifyState("threatened", 1);
                CancelCurrentGoal();
            }
            Invoke("LookForHostiles", statusCheck);
        }
    }


}
