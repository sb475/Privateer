using GameDevTV.Utils;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using RPG.Combat;
using RPG.Items;
using RPG.Control;
using System.Collections.Generic;
using RPG.Global;

namespace RPG.Stats
{
    public class CharacterStats : MonoBehaviour, IStat
    {
        [Header("Progression")]
        [Range(1, 10)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] GameObject levelUpParticleEffect = null;

        [Header("Character")]
        [SerializeField] CharacterClass characterClass;
        [SerializeField] CharacterRace characterRace;



        [Header("Character Stats")]

        [SerializeField] int majorAttributePoints;
        [SerializeField] int minorAttributePoints;
        public List<Stat> characterBaseStat;
        public StatList characterBaseStatData;

        public event Action onLevelUp;

       LazyValue<int> currentLevel;

        Experience experience;

        private void Awake() 
        {
            experience = GetComponent<Experience>();
            characterBaseStat = characterBaseStatData.statList;
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

       private void Start() 
       {
            currentLevel.ForceInit();
       }
        public float GetStat(StatName stat)
        {
            return (GetBaseStatValue(stat) + GetAdditiveModifier(stat)) * (1 + GetPercetangeModifier(stat)/100);
        }

        public float CalculateDamage(StatName stat, WeaponConfig weaponConfig)
        {
            return ((GameEvents.instance.GetRollValue((int)weaponConfig.GetDamage()) + GetBaseStatValue(stat) + GetAdditiveModifier(stat)) * (1 + GetPercetangeModifier(stat) / 100)) + weaponConfig.GetDamageBonus();
        }

        public float GetDamage(StatName stat, WeaponConfig weaponConfig)
        {
            return (weaponConfig.GetDamage() + GetBaseStatValue(stat) + GetAdditiveModifier(stat)) * (1 + GetPercetangeModifier(stat) / 100);
        }
        

        private float GetBaseStatValue(StatName stat)
         {
            foreach (Stat baseStat in characterBaseStat)    
            {
                if (baseStat.statType.name== stat.ToString())
                {
                    return baseStat.baseValue;
                }
            }
            
            return 0;
        }
        private Stat GetBaseStat(StatName stat)
        {
            foreach(Stat baseStat in characterBaseStat)
            {
                if (baseStat.statType.name == stat.ToString())
                {
                    return baseStat;
                }
              
            }
            return null;
        }


        public int GetLevel()
       {
           return currentLevel.value;
       }

       public void spendMinor(int value, StatName stat)
       {
           if (minorAttributePoints > 0)
           {
               minorAttributePoints -= value;

               //GetBaseStat(stat).AddToBaseStat(value);
               GameEvents.instance.OnMinorAttribPoolChange(minorAttributePoints);

            }
            else
            {
                GameEvents.instance.SendMessage("You do not have enough points to spend");
            }

       }

        public void spendMajor(int value, StatName stat)
        {
            if (majorAttributePoints > 0)
            {
                majorAttributePoints -= value;

                //GetBaseStat(stat).AddToBaseStat(value);
                GameEvents.instance.OnMajorAttribPoolChange(majorAttributePoints);
            }
            else
            {
                GameEvents.instance.SendMessage("You do not have enough points to spend");
            }
        }


        private float GetAdditiveModifier(StatName stat)
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

            if (GetComponent<CharacterPerks>() != null)
            {
                CharacterPerks perkStats = GetComponent<CharacterPerks>();

                foreach (float modifier in perkStats.GetAdditiveModifiers(stat))
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


        private float GetPercetangeModifier(StatName stat)
        {
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
      

       public int CalculateLevel()
       {
            return 1;
            //int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass); find this somewhere else
          
       }

    }

}
