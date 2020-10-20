using UnityEngine;
using static RPG.Control.RPG_TaskSystem;

namespace RPG.Control
{
    [CreateAssetMenu(fileName = "CharacterActions", menuName = "Privateer/CharacterActions", order = 0)]
    public class TaskableActions : ScriptableObject {

        public Task.Default task;

        public virtual Task Action()
        {
            return task;
        }
    
        
    }
}