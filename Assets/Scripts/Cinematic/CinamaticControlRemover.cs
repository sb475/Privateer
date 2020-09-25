using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematic
{
    public class CinamaticControlRemover : MonoBehaviour
    {
        GameObject player;

        private void Awake() 
        {
            player = GameObject.FindWithTag("Player");
        }

        private void OnEnable() 
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void OnDisable()
        {
            GetComponent<PlayableDirector>().played -= DisableControl;
            GetComponent<PlayableDirector>().stopped -= EnableControl;
        }

        void DisableControl(PlayableDirector pd)
        {
            
            player.GetComponent<ActionScheduler>().CancelCurrectAction();
            player.GetComponent<CrewController>().enabled = false;
        }

        void EnableControl(PlayableDirector pd)
        {
            player.GetComponent<CrewController>().enabled = true;
        }
        
        
    }

}