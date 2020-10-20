using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.GameState
{
    public class GameStateTextSpawner : MonoBehaviour
    {
        [SerializeField] GameStateText GameStateTextPrefab = null;

        
        public void Spawn(string gameState)
        {
        
           GameStateText instance = Instantiate<GameStateText>(GameStateTextPrefab, transform);
           instance.SetValue(gameState);
        }

    }
}