using UnityEngine;

public class ToggleTacticOptions : MonoBehaviour
{

    public void ShowOptions(GameObject tacticsOptions)
    {
        if (tacticsOptions.activeSelf == true)
        {
            tacticsOptions.SetActive(false);
        }
        else if (tacticsOptions.activeSelf == false)
        {
            tacticsOptions.SetActive(true);
            OptionsOpenAnimation();
        }
    }

    public void OptionsOpenAnimation()
    {
        GetComponent<Animator>().SetBool("Run",true);
    }

}
