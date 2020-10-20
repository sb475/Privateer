using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.Items;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Global {
    //this needs to get converted into a state machine or something like it.
    
    public enum GameState { OUTOFCOMBAT, COMBAT, COMBATOVER, GAMEOVER };

    public class WorldController : MonoBehaviour
    {

        public static WorldController instance;

        [SerializeField] public GameState state;
        bool announce;
        public CoverObject [] coverObjects;

        [SerializeField] StateChangeEvent stateChangeEvent;
        [System.Serializable]
        public class StateChangeEvent : UnityEvent<string>
        {

        }

        [SerializeField] GameObject battleController;

        private void Awake() {
            instance = this;
        }

        private void UpdateGameState(object sender, GameState e)
        {
            state = e;
            stateChangeEvent.Invoke(ChangeStateText(state));
        }
        // Start is called before the first frame update
        void Start() {
            GameEvents.instance.OnGameStateChange += UpdateGameState;
            state = GameState.OUTOFCOMBAT;
            announce = true;

            coverObjects = GameObject.FindObjectsOfType<CoverObject>();
        }

        // Update is called once per frame
        void Update()
        {
            switch(state)
            {
            case GameState.COMBAT:
                if (announce){
                announce = false;
                }
                break;
            case GameState.OUTOFCOMBAT:
                break;
            case GameState.GAMEOVER:
                break;
            }

        }

        public CoverObject [] GetCoverSpots()
        {
            return coverObjects;
        }
       
        private string ChangeStateText(GameState gameState)
        {
            if (gameState == GameState.OUTOFCOMBAT) return "OUT OF COMBAT";
            return state.ToString();
        }

        public GameState GetState()
        {
            return state;
        }

    }
}
