using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RPG.UI
{
    [System.Serializable]
    public class CharacterDetails
    {
    public GameObject characterPrefab;
    public string characterDescription;
    public string characterRace;
    }

    public class CharacterSelection : MonoBehaviour
    {
        public List<CharacterDetails> characterOptions;
        public CharacterDetails currentOption;

        public Text displayCharacterDescription;
        public Text displayCharacterRace;

        

        public int currentOptionIndex = 0;

        private void Awake()
        {
            currentOption = characterOptions[currentOptionIndex];
        }

        private void Update() {
            displayCharacterDescription.text = currentOption.characterDescription;
            displayCharacterRace.text = currentOption.characterRace;
        }

        public void CycleLeft ()
        {
            CharacterSelection character = GameObject.Find("Selection").GetComponent<CharacterSelection>();
            int index = character.currentOptionIndex;
            List<CharacterDetails> characterOptions = character.characterOptions;

            character.currentOption.characterPrefab.SetActive(false);
      
            if ( index == 0)
            {
                character.currentOptionIndex = (character.characterOptions.Count - 1);
            }
            else 
            {
                character.currentOptionIndex -=1;
            }
            character.currentOption = characterOptions[character.currentOptionIndex];
            character.currentOption.characterPrefab.SetActive(true);

        }
        public void CycleRight()
        {
            CharacterSelection character = GameObject.Find("Selection").GetComponent<CharacterSelection>();
            int index = character.currentOptionIndex;
            List<CharacterDetails> characterOptions = character.characterOptions;

            character.currentOption.characterPrefab.SetActive(false);

            //if character selection reaches top of array, reset to 0
            if (index == character.characterOptions.Count - 1)
            {
                character.currentOptionIndex = 0;
            }
            else
            {
                character.currentOptionIndex += 1;
            }
            character.currentOption = characterOptions[character.currentOptionIndex];
            character.currentOption.characterPrefab.SetActive(true);

        }

        // SelectCharacter();

        // ExportCharacterToOrk();
    }

}
