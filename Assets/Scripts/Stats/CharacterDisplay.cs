using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class CharacterDisplay : MonoBehaviour
    {
        [SerializeField] Text experienceText;
        [SerializeField] Text healthText;
        [SerializeField] Text levelText;
        
        Health health;
        Experience experience;
        BaseStats baseStats;

        private void Awake() 
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>(); //this will find gameobject with tag and look at health component.
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            
        }
        private void Update() 
        {
            healthText.text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints()); // gets the percentage of health from health.cs and updates the "text" component. String format changes way text is displayed.
            experienceText.text = String.Format("{0:0}", experience.GetPoints());
            levelText.text = String.Format("{0:0}", baseStats.CalculateLevel());
        }
        
    }

}