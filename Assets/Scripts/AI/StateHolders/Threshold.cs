using RPG.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Threshold : MonoBehaviour
{

    public Room roomA;
    public Room roomB;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GAgent>() != null)
        {
            GAgent agent = other.GetComponent<GAgent>();
            Room currentRoom = agent.GetCurrentRoom();

            if (currentRoom == roomA)
            {
                agent.SetCurretRoom(roomB);
            }
            else if (currentRoom == roomB)
            {
                agent.SetCurretRoom(roomA);
            }
        }
    }


}
