using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class TargetDisplay : MonoBehaviour
    {
        [SerializeField] Text targetHealth;
        [SerializeField] GameObject targetOverview;
       // [SerializeField] Text targetClass;
        Fighter fighter;
        GameObject targetWindow;
        //Add if null variable to make it so that the menu (UI component) is not displayed at all.


        private void Awake() 
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>(); //this will find gameobject with tag and look at health component.
            GameObject targetWindow = GameObject.Find("TargetWindow");
        }
        private void Update() 
        {
            
            SetTargetOverview();
        
        }
        public void SetTargetOverview()
        {
            //Impliment for raycasting

            if (fighter.GetTarget() == null)
            {
                targetOverview.SetActive(false); // Deactivate UI menu item "TargetWindow", remove to player screen
                                                 // targetHealth.text = "N/A";
                return;
            }
            targetOverview.SetActive(true); // Activate UI menu item "TargetWindow"
            Health health = fighter.GetTarget();
            targetHealth.text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints()); ; // gets the percentage of health from health.cs and updates the "text" component. String format changes way text is displayed.
        }
        
    }
}

