
using System.Collections.Generic;
using RPG.Global;
using RPG.Stats;
using RPG.UI;
using UnityEngine;
using static RPG.Global.GameEvents;

namespace RPG.Combat
{    
    public class BattleTactics : MonoBehaviour, IModifierProvider
    {
        string statOwner;
        public TacticsOptions activeBattleTactic;

        [SerializeField] Modifier[] aggresiveModifiers;
        [SerializeField] Modifier[] defensiveModifiers;
        [SerializeField] Modifier[] tacticalModifiers;
        [SerializeField] Modifier[] noModifiers;

        [SerializeField] Modifier[] activeModifiers;
    
        [System.Serializable]
        struct Modifier
        {
            public Stat stat;
            public float value;
        }

        private void Awake()
        {
            statOwner = "CrewController";
            activeBattleTactic = TacticsOptions.None;
            activeModifiers = noModifiers;
        }

        private void Start() {
            GameEvents.instance.battleTacticChanged += UpdateBattleTactics;
        }

        private void UpdateBattleTactics(object sender, BattleTacticOptions tactic)
        {
            Debug.Log ("BattleTactic updated to " + tactic.tactic);
            GetBattleTactics(tactic.tactic);
        }


       private void GetBattleTactics(TacticsOptions tacticsOptions)
       {

           activeBattleTactic = tacticsOptions;

            if (tacticsOptions == TacticsOptions.Aggressive)
            {
                activeModifiers = aggresiveModifiers;
                
            }
            else if (tacticsOptions == TacticsOptions.Defensive)
            {
                activeModifiers = aggresiveModifiers;
            }
            else if (tacticsOptions == TacticsOptions.Tactical)
            {
                activeModifiers = tacticalModifiers;
            }
            else if (tacticsOptions == TacticsOptions.None)
            {
                activeModifiers = noModifiers;
            }
        }


        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            yield return 0;
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            Debug.Log("BattleTactics " + stat);
            if (activeModifiers == null || activeModifiers == noModifiers) yield return 0;

            foreach (var modifier in activeModifiers)
            {
                if (modifier.stat == stat)
                {
                    Debug.Log(modifier.stat + " is % " + modifier.value);
                    yield return (modifier.value);
                }
            }
        }

    }
}