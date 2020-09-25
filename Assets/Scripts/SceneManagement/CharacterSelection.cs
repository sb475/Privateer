using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class CharacterSelection : MonoBehaviour
    {
        GameObject orcLight;
        GameObject orcMedium;
        GameObject orcHeavy;
        GameObject elfLight;
        GameObject elfMedium;
        GameObject elfHeavy; 
        GameObject humanLight;
        GameObject humanMedium;
        GameObject humanHeavy;
        int characterInt = 1;

    private void Awake() {

    }
    private void NextCharacter()
    {
        switch(characterInt)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;

        }
        
    }
        private void PreviousCharacter()
        {
            switch (characterInt)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                default:
                    break;

            }

        }
    }
}
