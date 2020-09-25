using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.GameState
{
    public class GameStateText : MonoBehaviour
    {
        [SerializeField] Text gameStateText = null;

        public void DestroyText()
        {
            Destroy(gameObject);
        }

        public void SetValue(string text)
         {
             gameStateText.text = String.Format("{0:0}", text);
         }

    }
}
