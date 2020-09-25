
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    //need to revisit with better event handlers

    
    public enum TacticsOptions
    {
        Aggressive,
        Defensive,
        Tactical,
        None
    }
public class SelectedTacticsOption : MonoBehaviour

    {
        public Image displaySelectedTactic;
        public TacticsOptions tacticsOptions;

        private void Awake() {
            tacticsOptions = TacticsOptions.None;
            
        }

        private void OnEnable() {
        tacticsOptions = TacticsOptions.None;
        }

        public TacticsOptions GetTacticsOption()
        {
            return tacticsOptions;
        }

    }
}

    