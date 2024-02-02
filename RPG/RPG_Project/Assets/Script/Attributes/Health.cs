using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70;
        [SerializeField] TakeDamageEvent takeDamagehealth;
        [SerializeField] UnityEvent onDie;
        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        LazyValue<float> healthPoint;
        bool isDead = false;

        private void Awake()
        {
            healthPoint = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public void Heal(float healthToRestore)
        {
            healthPoint.value = Mathf.Min(healthPoint.value + healthToRestore, GetMaxHealthPoints());
        }

        private void Start()
        {
            healthPoint.ForceInit();
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        public float GetHealthPoints()
        {
            return healthPoint.value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * regenerationPercentage / 100;
            healthPoint.value = Mathf.Max(healthPoint.value, regenHealthPoints);
        }

        public void takeDamage(GameObject instigator, float damage)
        {
            healthPoint.value = Mathf.Max(healthPoint.value - damage, 0);

            if(healthPoint.value == 0)
            {
                onDie.Invoke();
                Die();
                AwardExperiences(instigator);
            }
            else
            {
                takeDamagehealth.Invoke(damage);
            }
        }

        private void AwardExperiences(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if(experience == null)
            {
                return;
            }
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        public float GetPercentage()
        {
            return 100 * GetFraction();
        }

        public float GetFraction()
        {
            return healthPoint.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Die()
        {
            GetComponent<Animator>().SetTrigger("die");
            isDead = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        public bool IsDead()
        {
            return isDead;
        }

        public object CaptureState()
        {
            return healthPoint.value;
        }

        public void RestoreState(object state)
        {
            healthPoint.value = (float)state;

            if (healthPoint.value <= 0)
            {
                Die();

            }
        }
    }
}
