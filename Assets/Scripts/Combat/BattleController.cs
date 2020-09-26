using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using RPG.Stats;
using System.Linq;
using RPG.Attributes;
using RPG.Control;
using RPG.Global;

namespace RPG.Combat
{
    public class BattleController : MonoBehaviour
    {
        public event EventHandler<EventArgs> combatStarted;
        public event EventHandler<EventArgs> combatEnded;

        public PlayerController crewController;

        [Header("Combatant tracking")]
        public List<GameObject> combatantsInRange = new List<GameObject>();
        public List<GameObject> friendlies = new List<GameObject>();
        public List<GameObject> enemies = new List<GameObject>();

        [Header("Iniate battle variables")]
        public float aggroRadius = 10f;
        public Vector3 colliderPosition;

        [Header("In battle variables")]
        public GameObject activeCombatant;
        Fighter fighter;
        StateManager turnManager;
        public bool battle;
        Vector3 lastLocation;

        private LineRenderer lineRenderer;

        [SerializeField] float moveDistance;
        [SerializeField] bool stopMoveCheck;
        [SerializeField] int combatantIndex;
        [SerializeField] bool turnOverMessage = false;
        [SerializeField] bool battleOverMessage = false;

        //Whole script needs to be converted into coroutines

        private void Awake()
        {
            //this checks to see if object already exists, if it does then it deletes any further attempts to duplicate

            battle = false;

        }

        private void Start() {
            
            GameEvents.instance.OnFightBrokeOut += CombatHasBeenInitated;
            GameEvents.instance.OnCharacterDeath += CharacterDied;
        }

        private void CharacterDied(object sender, GameObject e)
        {
            if (battle)
            {
                if (enemies.Contains(e) == true)
                {
                    enemies.Remove(e);
                }
                else if (friendlies.Contains(e))
                {
                    friendlies.Remove(e);
                }
                combatantsInRange.Remove(e);

                //cross out character on screen gui
            }
        }

        public void CombatHasBeenInitated (object sender, Vector3 e)
        {
            Debug.Log("CombatHasBeenInitated");
            //StopAllCoroutines();
            StartCoroutine(InitiateCombat(e));
        }

#region InitializeCombat
        public IEnumerator InitiateCombat (Vector3 aggressorPosition)
        {
            //Determine who is in combat
            yield return new WaitUntil(() => GetCombatants(aggressorPosition, aggroRadius));
            Debug.Log(combatantsInRange.Count);

            //get all of the friends in surrounding area into combat as well.
            yield return new WaitUntil(() => GetNearbyCombatants());

            Debug.Log(combatantsInRange.Count);

            //Set combtatants state and cancel any actions
            yield return new WaitUntil(() => PlaceCombatantsIntoCombat());

            //Find out who goes first and sort t)he array
            yield return new WaitUntil(() => SortByTurn());

            BeginCombatt();

            //Create function to allow player to move and do things
            
            UpdateGameState(GameState.COMBAT);

            //Create events
        }

        #region GetObjectsIntoCombatArray
        private bool GetCombatants(Vector3 center, float radius)
        {
            Collider[] hitColliders = Physics.OverlapSphere(center, radius);

            foreach (var hitCollider in hitColliders)
            {
                if (hitColliders != null)
                {

                    //Handle colliders and add them to combat array as well as friendly or enemy arrays.
                    //Finds only AI elements in collider range
                    InitializeNPCCombatants(hitCollider);

                    //Need to determine if want all crew to automatically engage in combat or if they can be brought in later
                    InitializeCrewCombatants(hitCollider);
                    //List every item in foreach loop
                    //Debug.Log(hitCollider.gameObject);
                }

            }
            //whole team will go into combat. Need to add fork to place "InRangeOfCombat". Characters outside of combat range will use AP to act but only spend at 1.5x.
            return true;
        }

        private void InitializeNPCCombatants(Collider hitCollider)
        {
            if ((hitCollider.gameObject.GetComponent(typeof(AIController)) as AIController) != null)
            {
                //Ensure that element does not already exist in Collider array
                if (combatantsInRange.Contains(hitCollider.gameObject) == false)
                {
                    //Checks to make sure they are aggressive before placing them into combat
                    if (hitCollider.gameObject.GetComponent<AIController>().GetAttitude() == AttitudeType.Hostile)
                    {
                        combatantsInRange.Add(hitCollider.gameObject);
                        enemies.Add(hitCollider.gameObject);
                    }
                    if (hitCollider.gameObject.GetComponent<AIController>().GetAttitude() == AttitudeType.Friendly)
                    {
                        combatantsInRange.Add(hitCollider.gameObject);
                        friendlies.Add(hitCollider.gameObject);
                    }
                    //Debug.Log(hitCollider.gameObject + " added to combatantsInRange");
                }
            }
        }
        private void InitializeCrewCombatants(Collider hitCollider)
        {
            if ((hitCollider.gameObject.GetComponent(typeof(CrewMember)) as CrewMember) != null)
            {
                //Ensure that element does not already exist in Collider array
                if (combatantsInRange.Contains(hitCollider.gameObject) == false)
                {
                    combatantsInRange.Add(hitCollider.gameObject);
                    friendlies.Add(hitCollider.gameObject);
                }
            }
        }
        #endregion

        #region SetCombatStates
        private bool PlaceCombatantsIntoCombat()
        {
            for (int i = 0; i < combatantsInRange.Count; i++)
            {

                combatantsInRange[i].GetComponent<StateManager>().InitializeCombatStates();
                print(combatantsInRange[i] + "is in combat");
            }
            return true;
        }
        #endregion

        private bool GetNearbyCombatants()
        {
            for (int i = 0; i < combatantsInRange.Count; i++)
            {
                GetCombatants(combatantsInRange[i].transform.position, aggroRadius);
            }
            return true;
        }

        private void BeginCombatt()
        {
            combatantIndex = 0;
            battle = true;
            BeginTurn(combatantIndex);
            
        }
#endregion



        // private void PrepareLineRendere()
        // {
        //     lineRenderer = gameObject.AddComponent<LineRenderer>();
        //     lineRenderer.startWidth = 0.2f;
        //     lineRenderer.enabled = false;
        // }

        private void Update() {
            if (battle)
            {
                TrackCombatMovement(activeCombatant);
                CheckTurnStatus();
                CheckWinCondition();
            }
        }

        private void CheckTurnStatus()
        {
            //will need to add check in here incase there are left over Action Points but not actions possible. Build into AI development
            if (turnManager.TurnOver())
            {
                Debug.Log(activeCombatant + "'s turn is over, there's nothing else that can be done.");
                    if (activeCombatant.GetComponent<CrewMember>() != null)
                    {
                        turnOverMessage = true;
                        return;
                    }
                    else
                    {
                        turnOverMessage = true;
                        EndTurn();
                    }
            }
        
        }
        //Determines if combat has ended
        public void CheckWinCondition()
        {
            if (enemies.Count < 1)
            {
                foreach (GameObject friend in friendlies)
                {
                    battle = false;
                    friend.GetComponent<StateManager>().SetToNonCombat();
                    UpdateGameState(GameState.OUTOFCOMBAT);
                    //GameEvents.instance.
                }
                Debug.Log("Combat won");
            }
            else if (friendlies.Count < 1)
            {
                //Game over event
                Debug.Log("Combat lost");
                UpdateGameState(GameState.GAMEOVER);
            }       
        }

#region TrackMovementInCombat

        private void TrackCombatMovement(GameObject currentCombatant)
        {
            if (currentCombatant == null) return;
            
            if (turnManager.GetCanMove())
            {
                float distance = 0f;

                //if no change in position do not ipdate
                if (lastLocation != currentCombatant.transform.position)
                {
                    if (moveDistance > 0f)
                    {
                        distance += Mathf.Round((Vector3.Distance(lastLocation, currentCombatant.transform.position)) * 100f) / 100f;
                        if (!Mathf.Approximately(distance, 0))
                        {
                            moveDistance -= distance;
                            lastLocation = currentCombatant.transform.position;
                        }
                    }
                    else
                    {
                        turnManager.SetCanMove(false);
                        //  Makes sure that cached commands are stopped once CanMove is over.
                        currentCombatant.GetComponent<Fighter>().Cancel();
                        Debug.Log(currentCombatant + " can no longer move.");
                        stopMoveCheck = true;
                    }
                }
            }
        }

#endregion

#region TurnBehaviour
        private void BeginTurn(int i)
        {
            
            SetActiveCombatant(activeCombatant = combatantsInRange[i]);
            Debug.Log("It is " + activeCombatant+"'s turn");

            lastLocation = activeCombatant.transform.position;
            turnOverMessage = false;

        }

        private void SetActiveCombatant(GameObject setCombatant)
        {
            if (setCombatant.GetComponent<CrewMember>() != null)
            {
                crewController.SetCrewMember(setCombatant.GetComponent<CrewMember>());
            }

            turnManager = setCombatant.GetComponent<StateManager>();
            moveDistance = setCombatant.GetComponent<BaseStats>().GetStat(Stat.Speed);
            fighter = setCombatant.GetComponent<Fighter>();
            
            //set combant to active turn
            turnManager.SetActiveTurn(true);
            RestoreActiveCombatantAP(activeCombatant);
        }

        private void RestoreActiveCombatantAP(GameObject currentCombatant)
        {
            Fighter inCombatFighter = currentCombatant.GetComponent<Fighter>();
            inCombatFighter.RestoreActionPoints();
        }


        public void EndTurn()

        {            //This is using different allocation because it's being called by a button
            turnManager.SetActiveTurn(false);
            NextCombatant();
        }
#endregion

        private void NextCombatant()
        {
            if ((combatantIndex + 1) > (combatantsInRange.Count -1))
            {
                combatantIndex = 0;
                BeginTurn(combatantIndex);
            }
            else if (combatantsInRange[combatantIndex + 1] == null)
            {
                combatantIndex = 0;
                BeginTurn(combatantIndex);
            }
            else if (combatantsInRange[combatantIndex + 1].GetComponent<Health>().IsDead())
            {
                BringOutYourDead(combatantsInRange[combatantIndex + 1]);
                //Events for end of round
                combatantIndex++;
                NextCombatant();
            }
            else{
                combatantIndex ++;
                BeginTurn(combatantIndex);
            }
        }

        private void BringOutYourDead(GameObject nextcombatant)
        {
            combatantsInRange.Remove(nextcombatant);
            foreach( GameObject friend in friendlies)
            {
                if (friend == nextcombatant)
                {
                    friendlies.Remove(nextcombatant);
                }
            }
            foreach (GameObject enemy in enemies)
            {
                if (enemy == nextcombatant)
                {
                    enemies.Remove(nextcombatant);
                }
            }
            

        }



#region DetermineTurnOrder
        private bool SortByTurn()
        {
            //Compares combatants speed variable. If it's tied it will favor the player.
            combatantsInRange = combatantsInRange.OrderByDescending(x => x.GetComponent<BaseStats>().GetStat(Stat.Speed)).ThenByDescending(x => x.GetComponent<PlayerController>()).ToList();
            return true;
        }
#endregion
        //Changes game state to COMBAT
        
        
        public void UpdateGameState(GameState state)
        {
            //Guard statement
            //if (player.GetComponent<StateManager>().isInCombat == false) return;

            GameEvents.instance.GameStateChange(state);
            print(state);
        }





        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(colliderPosition, aggroRadius);
        }
    



    }
}