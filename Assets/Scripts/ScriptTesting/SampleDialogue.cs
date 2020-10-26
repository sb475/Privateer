using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SampleDialogue : MonoBehaviour
{
    [TextArea(5, 20)]
    public string [] conversation;
    public TextMeshProUGUI conversationText;
    int conversationIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        //HaveConversation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void NextDialogue()
    {
        conversationText.text = conversation[1];
    }

    public void HaveConversation()
    {
        conversationText.text = conversation[conversationIndex];
        conversationIndex++;
    }
}
