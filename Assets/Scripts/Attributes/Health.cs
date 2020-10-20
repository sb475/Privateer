using UnityEngine;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;
using GameDevTV.Utils;
using UnityEngine.Events;
using RPG.Global;
using RPG.Combat;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable, IDamagable
    {
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent onDie;
        [SerializeField] UnityEvent hasBeenAttacked;
        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        LazyValue<Health> health;

        
        public float maxHealth;
        public float currentHealth;
        public bool isDead = false;
        
        private void Start() {
                maxHealth = GetInitialHealth();
                currentHealth = maxHealth;
        }
        private float GetInitialHealth()
        {
            return GetComponent<IStat>().GetStat(StatName.Health);
        }
        public bool IsDead()
        {
            return isDead;
        }

        public bool RollToHit(GameObject instigator)
        {
            float targetArmorClass = GetComponent<IStat>().GetStat(StatName.Armor);
            //add base stats to combat
            float hitRollValue = GameEvents.instance.GetRollValue(20);
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
            GetAttacked();

            if (!RollToHit(instigator)) return;

            print(gameObject.name + " took damage: " + damage);
            currentHealth = Mathf.Max(currentHealth - damage, 0);
                // takeDamage.Invoke(damage);
                if (currentHealth == 0) 
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
        public void GetAttacked ()
        {
            hasBeenAttacked.Invoke();
            Debug.Log(gameObject + " has been attacked");
        }

        public float GetHealthPoints()
        {
            return currentHealth;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<CharacterStats>().GetStat(StatName.Health);
        }

        public float GetPercentage()
        {
            return 100 * (GetFraction());

        }

        public float GetFraction()
        {
            return currentHealth / maxHealth;
        }

        public void Die()
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

            experience.GainExperience(GetComponent<CharacterStats>().GetStat(StatName.ExperienceReward));
        }

        public void Heal (int x)
        {
            float amountHealed = currentHealth + (float)x;

            Debug.Log (x);
            Debug.Log (currentHealth);
            if (amountHealed > GetInitialHealth())
            {
                amountHealed = GetInitialHealth() - currentHealth;
                currentHealth = GetInitialHealth();
            }
            else
            {
                amountHealed = x;
            }

            Debug.Log ("You were healed for " + amountHealed);

        }

        public object CaptureState()
        {
            return currentHealth;
        }

        public void RestoreState(object state)
        {
            currentHealth = (float)state;

            if (currentHealth == 0)
            {
                Die();

            }
        }

        public void TargetLocked(Missile missile)
        {
            throw new System.NotImplementedException();
        }
    }
}