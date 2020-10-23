using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.AI
{
    [ExecuteInEditMode]
    public class CharacterVisual : MonoBehaviour
    {
        public Character thisChar;

        // Start is called before the first frame update
        void Start()
        {
            thisChar = this.GetComponent<Character>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
