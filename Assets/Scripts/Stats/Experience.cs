using UnityEngine;
using RPG.Saving;
using System;

namespace RPG.Stats
{
    
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;

       // public delegate void ExperiencedGainedDelegate();
        public event Action onExperiencedGained;
        
        public void GainExperience (float experience)
        {
            experiencePoints += experience;
            onExperiencedGained();
        }

        public object CaptureState()
        {
            return experiencePoints;
        }
        public void RestoreState(object state)
        {
            experiencePoints = (float)state;        
        }

        public float GetPoints()
        {
            return experiencePoints;
        }
    }   

}