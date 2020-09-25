using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

public class StateManager : MonoBehaviour
{
    public bool isInCombat = false;
    public bool hasActiveTurn = true;
    public bool canAct = true;
    public bool canMove = true;
    public bool isAlert = false;
    ActionScheduler actionScheduler;

    private void Awake() {
        actionScheduler = GetComponent<ActionScheduler>();
        hasActiveTurn = true;
    }

    public void InitializeCombatStates()
    {
 
        isInCombat = true;


        hasActiveTurn = false;
 

        canAct = false;


        canMove = false;


        FreezeAction();


    }
    
    // Start is called before the first frame update
    public bool GetCanMove()
    {
        return canMove;
    }
    public bool HasActiveTurn()
    {
        return hasActiveTurn;
    }
    public bool GetCanAct()
    {
        return canAct;
    }

    public bool TurnOver()
    {
        if (canAct && canMove)
        {
            return false;
        }
        return true;
    }

    public void FreezeAction()
    {
        actionScheduler.CancelCurrectAction();
    }
     
    public void SetActiveTurn(bool status)
    {
        hasActiveTurn = status;
        canMove = status;
        canAct = status;
    }
    public void SetCanMove(bool status)
    {
        canMove = status;
    }
    public void SetCanAct(bool status)
    {
        canAct = status;
    }
    public void SetToNonCombat()
    {
        canAct = true;
        canMove = true;
        hasActiveTurn = true;
        isInCombat = false;
    }
}
