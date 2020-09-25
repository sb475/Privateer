
using System;
using RPG.Base;
using RPG.Control;
using RPG.Items;
using RPG.UI;
using UnityEngine;

namespace RPG.Events
{
    [System.Serializable]
    public class Dialogue
    {
        public string name;
        
        [TextArea(3, 10)]
        public string[] sentences;
        
      
    }
}