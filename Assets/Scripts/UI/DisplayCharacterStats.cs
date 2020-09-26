using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;
using RPG.Stats;
using UnityEngine.UI;
using RPG.Attributes;
using RPG.Combat;
using System;
using TMPro;
using RPG.Global;

namespace RPG.UI{
    
    public class DisplayCharacterStats : MonoBehaviour
    {
        [SerializeField] UIController uIController;
        [SerializeField] private BaseStats player;
        public GameObject container;
        public GameObject displayTemplate;
        public List<Stat> statsToDisplay;
        

        [SerializeField] TextMeshProUGUI currentHealth;
        [SerializeField] TextMeshProUGUI totalHealth;
        [SerializeField] TextMeshProUGUI displayArmor;
        [SerializeField] TextMeshProUGUI displayDamage;

        
        private void Awake() {
            
        }

        private void OnEnable() {
            GameEvents.instance.ItemChanged += EventGenerateDisplay;
            player = uIController.GetCrewToDisplay().GetComponent<BaseStats>();
            GenerateCharacterDisplay(player);
        }
        private void OnDisable() => GameEvents.instance.ItemChanged -= EventGenerateDisplay;

        private void EventGenerateDisplay (object sender, System.EventArgs e)
        {
            player = uIController.GetCrewToDisplay().GetComponent<BaseStats>();
            GenerateCharacterDisplay(player);
        }

        public bool GenerateCharacterDisplay(BaseStats playerToDisplay)
        {
            if (playerToDisplay == null) return false;
            RefreshCharacterDisplay();

            //look at decoupling this in the future
            currentHealth.text = playerToDisplay.GetComponent<Health>().GetHealthPoints().ToString();
            totalHealth.text = playerToDisplay.GetComponent<Health>().GetMaxHealthPoints().ToString();
            displayArmor.text = playerToDisplay.GetStat(Stat.Armor).ToString();
            displayDamage.text = playerToDisplay.GetComponent<Fighter>().DamageAsString();


            foreach (Stat stat in statsToDisplay)
            {
           
                RectTransform statToDisplayTransform = Instantiate(displayTemplate.transform, container.transform).GetComponent<RectTransform>();

                statToDisplayTransform.gameObject.SetActive(true);

                AttributeToDisplay attributeToDisplay = statToDisplayTransform.GetComponentInChildren<AttributeToDisplay>();
                AttributeValueToDisplay attributeValueToDisplay = statToDisplayTransform.GetComponentInChildren<AttributeValueToDisplay>();
                
                TextMeshProUGUI attributeToDisplayTextMeshProUGUI = attributeToDisplay.GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI attributeValueToDisplayTextMeshProUGUI = attributeValueToDisplay.GetComponent<TextMeshProUGUI>();

                attributeToDisplayTextMeshProUGUI.text = stat.ToString();
                attributeValueToDisplayTextMeshProUGUI.text = playerToDisplay.GetStat(stat).ToString();
            }
            return true;
        }

        public void RefreshCharacterDisplay()
        {

            foreach (Transform child in container.transform)
            {
                if (child == displayTemplate.transform)
                {
                    child.gameObject.SetActive(false);
                    continue;
                }
                Destroy(child.gameObject);
            }

        }

        
    }

}