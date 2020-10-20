using RPG.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Citizen : GAgent
{
    public float statusCheck = 0.5f;
    public GameObject hostile; 
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

        Invoke(nameof(LookForHostiles), statusCheck);

    }

    void LookForHostiles()
    {
        if (hostile == null) return;
        if (Vector3.Distance(hostile.transform.position, this.transform.position) < 15)
        {
            Debug.Log("in range");
           beliefs.ModifyState("threatened", 1);
           CancelCurrentGoal();
        }
        Invoke("LookForHostiles", statusCheck);
    }


}
