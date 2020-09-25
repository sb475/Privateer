﻿using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Base;
using RPG.Combat;
using RPG.Control;
using RPG.Core;
using RPG.Stats;
using RPG.UI;
using UnityEngine;
using UnityEngine.UI;
//This script controls a majority of game functionality and works as reference in order to easily call functions from many
//different components.

//When it comes to making ships, look at this video again: https://www.youtube.com/watch?v=mJRc9kLxFSk ~11min
namespace RPG.Global {
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


        public class BattleTacticOptions : EventArgs
        {
            public Sprite sprite;
            public TacticsOptions tactic;
        }

        
        public GameObject gameMenu;

        public GameObject equippedItemSlots;
        public GameObject playerObject;

        public CrewController crewController;
        internal bool battleEventCalled;


        private void Awake() {
            instance = this;
            battleEventCalled = false;
            
        }

        private void Start() {
            playerObject = crewController.GetSelectedCrewMember().gameObject;
            UpdateSelectedCrew(playerObject.GetComponent<CrewMember>());

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

#endregion




        #region ActionMenu
        public void SelectActionMenu(ActionMenuOptions selectedOptions, Interactable interactable)
        {

            //actions based on clicking from mouse. Most rely on Coroutine Act, which functionality 
            //is changed by editing the delagate "interactableAction". See in PlayerController.cs. Some 
            //actions require to be manually altered at this time.
     
            switch (selectedOptions)
            {

                case ActionMenuOptions.Attack:
                    crewController.interactableAction = crewController.Attack;
                    break;
                case ActionMenuOptions.Trade:
                    crewController.interactableAction = crewController.Trade;
                    break;
                case ActionMenuOptions.Talk:
                    crewController.interactableAction = crewController.Talk;
                    break;
                case ActionMenuOptions.Move:
                    crewController.interactableAction = crewController.Move;
                    break;
                case ActionMenuOptions.Open:
                    crewController.interactableAction = crewController.Open;
                    break;
                case ActionMenuOptions.PickUp:
                    crewController.interactableAction = crewController.PickUp;
                    break;
                case ActionMenuOptions.Inspect:
                    crewController.interactableAction = crewController.Inspect;
                    break;
                case ActionMenuOptions.Scan:
                    crewController.interactableAction = crewController.Scan;
                    break;
            }
            StartCoroutine(crewController.Act(crewController));

            ActionMenu.HideMenuOptions_Static();
        }

        public static void SelectActionMenu_Static(ActionMenuOptions selectedOptions, Interactable interactable)
        {
            instance.SelectActionMenu(selectedOptions, interactable);
        }

#endregion


#region CharacterEvents

        internal void BuyPerk(PerkType perkButtonType, int perkLevel)
        {
            Debug.Log("Adding perk " + perkButtonType);
            playerObject.GetComponent<CharacterStats>().AddPerk(Resources.Load<Perk>(perkButtonType.ToString()), perkLevel);
        }

        public void UpdateSelectedCrew(CrewMember newSelectedCrew)
        {

            playerObject = newSelectedCrew.gameObject;
            selectCrewChanged?.Invoke(this, newSelectedCrew);

        }

        public List<CrewMember> GetCrewRoster()
        {
            return crewController.ListCrew();
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