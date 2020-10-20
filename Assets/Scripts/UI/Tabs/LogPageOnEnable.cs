using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class LogPageOnEnable : PageOnEnable
{

    public QuestLogWindow runtimeQuestLogWindow;

    private void Awake() {
        runtimeQuestLogWindow = GetComponent<QuestLogWindow>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        runtimeQuestLogWindow.Open();
    }

    private void OnDisable() {
        runtimeQuestLogWindow.Close();
    }

}
