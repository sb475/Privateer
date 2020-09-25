using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Global {
    
    public enum GameState { OUTOFCOMBAT, COMBAT, COMBATOVER, GAMEOVER };

    public class WorldController : MonoBehaviour
    {

        [SerializeField] public GameState state;
        bool announce;

        [SerializeField] StateChangeEvent stateChangeEvent;
        [System.Serializable]
        public class StateChangeEvent : UnityEvent<string>
        {

        }

        [SerializeField] GameObject battleController;

        private void Awake() {
            GameEvents.instance.OnGameStateChange += UpdateGameState;
        }

        private void UpdateGameState(object sender, GameState e)
        {
            state = e;
            stateChangeEvent.Invoke(ChangeStateText(state));
        }
        // Start is called before the first frame update
        void Start() {
            
            state = GameState.OUTOFCOMBAT;
            announce = true;
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
