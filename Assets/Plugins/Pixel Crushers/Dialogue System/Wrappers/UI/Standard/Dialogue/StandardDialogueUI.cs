// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;

namespace PixelCrushers.DialogueSystem.Wrappers
{

    /// <summary>
    /// This wrapper class keeps references intact if you switch between the 
    /// compiled assembly and source code versions of the original class.
    /// </summary>
    [HelpURL("http://www.pixelcrushers.com/dialogue_system/manual2x/html/standard_dialogue_u_i.html")]
    [AddComponentMenu("Pixel Crushers/Dialogue System/UI/Standard UI/Dialogue/Standard Dialogue UI")]
    public class StandardDialogueUI : PixelCrushers.DialogueSystem.StandardDialogueUI
    {
        [SerializeField] GameObject blockingMask;

        public override void OnEnable()
        {
            base.OnEnable();
            blockingMask.SetActive(false);
        }

        public override void ShowSubtitle(Subtitle subtitle)
        {
            base.ShowSubtitle(subtitle);
            blockingMask.SetActive(true);
        }

        public override void HideSubtitle(Subtitle subtitle)
        {
            base.HideSubtitle(subtitle);
            blockingMask.SetActive(false);

        }
    }

}
