using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public SampleDialogue speaker1;
    public SampleDialogue speaker2;
    public bool firstSpeaker = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("First speaker is speaking, The value is: " + firstSpeaker);
            if (firstSpeaker)
            {
                speaker1.HaveConversation();
                firstSpeaker = false;
            }
            else
            {
                speaker2.HaveConversation();
                firstSpeaker = true;
            }

        }
    }
}
