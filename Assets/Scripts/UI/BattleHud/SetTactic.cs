using RPG.Control;
using RPG.Global;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class SetTactic : MonoBehaviour
    {
        public Image spriteToChangeTo;
        public TacticsOptions tacticSelection;

        
        public void NewTacticSelected ()
        {
            spriteToChangeTo = GetComponent<SetTactic>().spriteToChangeTo;
            tacticSelection = GetComponent<SetTactic>().tacticSelection;
            GameEvents.instance.OnBattleTacticChange(tacticSelection, spriteToChangeTo.sprite);
        }
       

    

    }
}