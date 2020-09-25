using UnityEngine;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;
using GameDevTV.Utils;
using UnityEngine.Events;
using RPG.Global;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regeneratePercentage = 70;
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent onDie;
        [SerializeField] UnityEvent hasBeenAttacked;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        LazyValue<float> healthPoints;

        bool isDead = false;
        
        private void Awake() 
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        public bool IsDead()
        {
            return isDead;
        }

        private void Start() 
        {
            healthPoints.ForceInit();

        }
        private void OnEnable() 
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }
        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }


        public bool RollToHit(GameObject instigator)
        {
            float targetArmorClass = GetComponent<BaseStats>().GetStat(Stat.Armor);
            //add base stats to combat
            float hitRollValue = GetComponent<BaseStats>().GetRollValue(20);
            print(hitRollValue);

            print("Roll to hit was: " + hitRollValue + ", " + instigator + " needed: " + targetArmorClass);
            if (hitRollValue < targetArmorClass)
            {
                if (hitRollValue == 1)
                {
                    print("catastrophic fail");
                    return false;
                }
                print(instigator + "'s attack missed");
                return false;

            }
            else
            {
                return true;
            }
        }
//this may also be where we impliment the roll for damage
        public void TakeDamage(GameObject instigator, float damage)
        {
            BeenAttacked();

            if (!RollToHit(instigator)) return;

            print(gameObject.name + " took damage: " + damage);
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
                // takeDamage.Invoke(damage);
                if (healthPoints.value == 0) 
                {
                    onDie.Invoke();
                    GameEvents.instance.CharacterDied(this.gameObject);
                    Die();
                    AwardExperience(instigator);                
                }
                else 
                {
                    takeDamage.Invoke(damage);
                }                     
        }
        public void BeenAttacked ()
        {
            hasBeenAttacked.Invoke();
            Debug.Log(gameObject + " has been attacked");
        }

        public float GetHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * (GetFraction());

        }

        public float GetFraction()
        {
            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Die()
        {
             if (isDead) return;

             isDead = true;         
             GetComponent<Animator>().SetTrigger("die");
             GetComponent<ActionScheduler>().CancelCurrectAction();

        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regeneratePercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints); // compares two numbers and picks the highest.
        }

        public void Heal (int x)
        {
            float amountHealed = healthPoints.value + (float)x;

            Debug.Log (x);
            Debug.Log (healthPoints.value);
            if (amountHealed > GetInitialHealth())
            {
                amountHealed = GetInitialHealth() - healthPoints.value;
                healthPoints.value = GetInitialHealth();
            }
            else
            {
                amountHealed = x;
            }

            Debug.Log ("You were healed for " + amountHealed);

        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;

            if (healthPoints.value == 0)
            {
                Die();

            }
        }

        public bool GetDeathStatus()
        {
            return isDead;
        }
    }
}