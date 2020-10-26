using System;
using System.Collections.Generic;
using RPG.Control;
using RPG.Stats;
using RPG.UI;
using UnityEngine;
using Random = UnityEngine.Random;
//This script controls a majority of game functionality and works as reference in order to easily call functions from many
//different components.

//When it comes to making ships, look at this video again: https://www.youtube.com/watch?v=mJRc9kLxFSk ~11min
namespace RPG.Global
{
    public class GameEvents : MonoBehaviour
    {

        public static GameEvents instance;
        
        public UIController uiController;
        public event EventHandler<EventArgs> ItemChanged;

       

        public event EventHandler<int> MinorAttribPoolChanged;
        public event EventHandler<int> MajorAttribPoolChanged;
        public event EventHandler<int> PerkPoolChanged;
        public event EventHandler<CrewMember> selectCrewChanged;
        public event EventHandler<BattleTacticOptions> battleTacticChanged;
        public event EventHandler<GameObject> OnCharacterDeath;
        public event EventHandler<GameState> OnGameStateChange;
        public event EventHandler<Vector3> OnFightBrokeOut;
        public event EventHandler<EventArgs> UpdateCrewList;


        public class BattleTacticOptions : EventArgs
        {
            public Sprite sprite;
            public TacticsOptions tactic;
        }

        
        public GameObject gameMenu;

        public GameObject equippedItemSlots;
        public GameObject playerObject;

        public PlayerController crewController;
        internal bool battleEventCalled;

        private void Awake() {
            instance = this;
            battleEventCalled = false;
            
        }

        private void Start() {
            playerObject = crewController.GetCurrentControllable().gameObject;

        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (gameMenu.activeSelf == true)
                {
                    gameMenu.SetActive(false);
                }
                else
                {                    
                    gameMenu.SetActive(true);
                }

            }
        }

#region  CombatEvents
        public void OnBattleTacticChange(TacticsOptions tacticsOptions, Sprite spriteToChangeTo)
        {
            battleTacticChanged?.Invoke(this, new BattleTacticOptions { sprite = spriteToChangeTo, tactic = tacticsOptions });
            Debug.Log ("OnBattleTacticChange");
        }

        public void CharacterDied(GameObject deadCharacter)
        {
            OnCharacterDeath?.Invoke(this, deadCharacter);
        }

        public void FightBreakOut(Vector3 fightLocation)
        {
            battleEventCalled = true;

            if (OnFightBrokeOut != null)
            {
                OnFightBrokeOut?.Invoke(this, fightLocation);
                Debug.Log("OnFightBreakOutt");
                uiController.DisplayBattleHud();
            }
        }

        public void OnCombatEnd()
        {

        }

#endregion

#region  GeneralEvents
    


        public void GameStateChange(GameState newState)
        {
            OnGameStateChange?.Invoke(this, newState);
            if (newState == GameState.OUTOFCOMBAT)
            {
                battleEventCalled = false;
            }
        }

        public void SendEventMessage(string message)
        {
            uiController.DisplayMessage(message);
        }

        public float GetRollValue(int rollValue)
        {
            int randomValue = Random.Range(1, rollValue);
            float rolledFloatValue = ConvertRollToFloat(randomValue);
            return rolledFloatValue;
            //lightd6, medium d8, heavy d10
        }

        private float ConvertRollToFloat(int roll)
        {
            float convertedRoll;
            convertedRoll = (int)roll;
            return convertedRoll;
        }
#endregion







#region CharacterEvents

        //internal void BuyPerk(PerkType perkButtonType, int perkLevel)
        //{
        //    Debug.Log("Adding perk " + perkButtonType);
        //    playerObject.GetComponent<CharacterPerks>().AddPerk(Resources.Load<Perk>(perkButtonType.ToString()), perkLevel);
        //}

        public List<CrewMember> GetCrewRoster()
        {
            return crewController.ListCrew();
        }
        public List<CrewMember> GetShipRoster()
        {
            return crewController.ListCrewOnShip();
        }


        public GameObject GetPlayerObject()
        {
            return playerObject;
        }

        public CrewMember GetPlayer()
        {
            return playerObject.GetComponent<CrewMember>();
        }

        public void OnItemChanged()
        {
            Debug.Log("GameEvent.OnItemChanged");
            ItemChanged?.Invoke(this, EventArgs.Empty);
        }

        public void OnMinorAttribPoolChange(int poolValue)
        {
            MinorAttribPoolChanged?.Invoke(this, poolValue);
        }
        public void OnMajorAttribPoolChange(int poolValue)
        {
            MajorAttribPoolChanged?.Invoke(this, poolValue);
        }
        public void OnPerkPoolChange(int poolValue)
        {
            PerkPoolChanged?.Invoke(this, poolValue);
        }




#endregion
 
       
    }

}
