using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace RPG.UI
{   
    public class LogPageOnEnable : PageOnEnable
    {
        private QuestLogWindow runtimeQuestLogWindow;

        public override void OnEnable()
        {
            base.OnEnable();
            runtimeQuestLogWindow.Open();
        }

    }

}
