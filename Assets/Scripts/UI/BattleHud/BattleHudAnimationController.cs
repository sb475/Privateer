using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{

    public class BattleHudAnimationController : MonoBehaviour
    {
        [SerializeField] GameObject showTacticBtn;
        [SerializeField] GameObject selectedTactic;
        [SerializeField] GameObject tacticOptionsBtn;
        [SerializeField] GameObject tacticOptionsImageBg;
        [SerializeField] float timeToAnimate = .2f;


        public void ShowOptions()
        {
            //if Tactics Menu is showing
            if (tacticOptionsBtn.activeSelf == true)
            {
                HideTacticsMenu();
            }
            //if Tactic Menu is not showing
            else if (tacticOptionsBtn.activeSelf == false)
            {
                tacticOptionsBtn.SetActive(true);
                LeanTween.scale(this.tacticOptionsBtn, new Vector3(1.5f, 1.5f, 1.5f), timeToAnimate);
                LeanTween.alphaCanvas(showTacticBtn.GetComponent<CanvasGroup>(), 0, timeToAnimate);
                LeanTween.rotateAroundLocal(tacticOptionsImageBg, Vector3.forward, 360f, 12f).setRepeat(-1);
                //LeanTween.moveLocalY(tacticOptionsBtn, 110f, timeToAnimate);
            }
        }

        private void HideTacticsMenu()
        {
            LeanTween.scale(tacticOptionsBtn, new Vector3(0.1f, 0.1f, 0.1f), timeToAnimate);
            LeanTween.alphaCanvas(showTacticBtn.GetComponent<CanvasGroup>(), 1, timeToAnimate);
            //LeanTween.moveLocalY(tacticOptionsBtn, -10f, timeToAnimate);
            //This will delay deactivating button until after 
            Invoke("SetActive", timeToAnimate);
        }

        public void SetActive()
        {
            tacticOptionsBtn.SetActive(false);
        }

        public void SelectedTacticsOption(GameObject btnResource)
        {

            selectedTactic.GetComponent<Image>().sprite = btnResource.GetComponent<SetTactic>().spriteToChangeTo.sprite;
            selectedTactic.GetComponent<SelectedTacticsOption>().tacticsOptions = btnResource.GetComponent<SetTactic>().tacticSelection;
            HideTacticsMenu();

        }




        //LeanTween.scale(avatarScale, new Vector3(1.7f, 1.7f, 1.7f), 5f).setEase(LeanTweenType.easeOutBounce);
        //LeanTween.moveX(avatarScale, avatarScale.transform.position.x + 5f, 5f).setEase(LeanTweenType.easeOutBounce);
    }

}