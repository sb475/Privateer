using System;
using RPG.Control;
using RPG.Global;
using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DisplayStatSlider: MonoBehaviour
    {
        public StatName statToDisplay;
        Slider statSlider;
        [SerializeField] bool isMajorAttribute;
        private CharacterStats characterBastStats;
        private CharacterStats characterStats;

        private void Awake() {
            GameEvents.instance.uiController.OnDisplayValueChange += OnEventDisplayCrewChange;
        }

        private void OnEnable()
        {
            statSlider = GetComponent<Slider>();
            DisplayAttributeSlider();

        }

        private void OnEventDisplayCrewChange(object sender, EventArgs e)
        {
            DisplayAttributeSlider();
        }

        public void DisplayAttributeSlider()
        {
            characterBastStats = GameEvents.instance.uiController.GetCrewToDisplay().GetComponent<CharacterStats>();
            GetComponent<Slider>().value = characterBastStats.GetStat(statToDisplay);
        }

        public void IncreaseStatFromUI (DisplayStatSlider slider)
        {
            Debug.Log ("button pushed");
            if (slider.isMajorAttribute)
            {
            //     if (GameEvents.instance.uiController.GetCrewToDisplay().GetComponent<CharacterStats>().SpendFromMajor(slider.statToDisplay))
            //     {

            //     }
            //     else
            //     {
            //         return;
            //     }
            // }
            // else{

            //     if (GameEvents.instance.uiController.GetCrewToDisplay().GetComponent<CharacterStats>().SpendFromMinor(slider.statToDisplay))
            //     {

            //     }
            //     else
            //     {
            //         return;
            //     }
            }
                slider.DisplayAttributeSlider();
        }

        public virtual bool CanIncreaseAttribute()
        {
            throw new NotImplementedException();
        }
    }
}