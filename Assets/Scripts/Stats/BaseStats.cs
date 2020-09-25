using GameDevTV.Utils;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using RPG.Combat;
using RPG.Items;
using RPG.Control;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Header("Progression")]
        [Range(1, 10)]
        [SerializeField] int startingLevel = 1;

        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpParticleEffect = null;

        [Header("Character")]
        [SerializeField] CharacterClass characterClass;
        [SerializeField] CharacterRace characterRace;
        
        [Header("Modifier")]
        [SerializeField] bool shouldUseModifier = false;

       public event Action onLevelUp;

       LazyValue<int> currentLevel;

        Experience experience;

        private void Awake() 
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

       private void Start() 
       {
            currentLevel.ForceInit();
            
        }
        
       private void OnEnable() 
       {
            if (experience != null)
            {
                experience.onExperiencedGained += UpdateLevel;
            }
       }

       private void OnDisable() 
       {
            if (experience != null)
            {
                experience.onExperiencedGained -= UpdateLevel;
            }
       }

       private void UpdateLevel() 
       {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
       }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercetangeModifier(stat)/100);
        }

        public float CalculateDamage(Stat stat, WeaponConfig weaponConfig)
        {
            return ((GetRollValue((int)weaponConfig.GetDamage()) + GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercetangeModifier(stat) / 100)) + weaponConfig.GetDamageBonus();
        }

        public float GetDamage(Stat stat, WeaponConfig weaponConfig)
        {
            return (weaponConfig.GetDamage() + GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercetangeModifier(stat) / 100);
        }
        

        public float GetRollValue(int rollValue)
        {
            int randomValue = Random.Range(1, rollValue);
            float rolledFloatValue = ConvertRollToFloat(randomValue);
            return rolledFloatValue;
            //lightd6, medium d8, heavy d10
        }

        private float ConvertRollToFloat(int roll)
        {
            float convertedRoll;
            convertedRoll = (int)roll;
            return convertedRoll;
        }


        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }


        public int GetLevel()
       {
           return currentLevel.value;
       }


        private float GetAdditiveModifier(Stat stat)
        {
            float total = 0;
            

            //local stats from armor

            if (GetComponent<CharacterEquipment>() != null)
            {
                CharacterEquipment characterEquipment = GetComponent<CharacterEquipment>();

                foreach (float modifier in characterEquipment.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }
            //local stats from perks

            if (GetComponent<CharacterStats>() != null)
            {
                CharacterStats characterStats = GetComponent<CharacterStats>();

                foreach (float modifier in characterStats.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }

            }

            //global stats like group buffs
            
            foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }


        private float GetPercetangeModifier(Stat stat)
        {
            if (!shouldUseModifier) return 0;

            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    Debug.Log (provider + " stat is " + stat + " and value is " + modifier);
                    total += modifier;
                }
            }
//            Debug.Log("Total is: " + total + " stat is " +stat);
            return total;
        }
      

       public int CalculateLevel()
       {
           experience = GetComponent<Experience>();

            if (experience == null) return startingLevel;

            float currentXP = experience.GetPoints();
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
               float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (XPToLevelUp > currentXP )
                {
                    return level;
                }   
            }
            return penultimateLevel + 1;
       }
    }

}
