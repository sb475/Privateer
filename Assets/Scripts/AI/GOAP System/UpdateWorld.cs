using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.AI
{
    public class UpdateWorld : MonoBehaviour
    {
        public StationAI station;
        public Text states;

        // Update is called once per frame
        void LateUpdate()
        {
            Dictionary<string, int> worldstates = GWorld.Instance.GetGOAPStates().GetStates();
            Dictionary<string, int> stationStates = station.states.GetStates();

            states.text = "";

            foreach (KeyValuePair<string, int> s in worldstates)
            {
                states.text += s.Key + ", " + s.Value + "\n";
            }
            foreach (KeyValuePair<string, int> s in stationStates)
            {
                states.text += s.Key + ", " + s.Value + "\n";
            }
        }
    }
}

