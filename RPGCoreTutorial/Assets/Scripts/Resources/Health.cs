using RPG.Core;
using RPG.Stats;
using RPG.Saving;
using UnityEngine;


namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenrationPercentage = 75;
        float healthPoints = -1f;

        bool isdead = false;


        void Start()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
            if (healthPoints < 0)
            {
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.HEALTH);
            }
        }

        public bool IsDead()
        {
            return isdead;
        }

        public float GetPercentage()
        {
            return healthPoints / GetComponent<BaseStats>().GetStat(Stat.HEALTH) * 100f;
        }

        public float GetHealthPts()
        {
            return healthPoints;
        }

        public float GetMaxHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.HEALTH);
        }

        public void TakeDamage(GameObject instigator, float _damage)
        {
            print(gameObject.name + " took damage : " + _damage);
            healthPoints = Mathf.Max(healthPoints - _damage, 0);
            if (healthPoints == 0f)
            {
                AwardExperience(instigator);
                Die();
            }
        }

        void AwardExperience(GameObject _instigator)
        {
            Experience experience = _instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.EXPERIENCE));
        }

        void RegenerateHealth()
        {
            float regenHealthPts = GetComponent<BaseStats>().GetStat(Stat.HEALTH) * regenrationPercentage / 100;
            healthPoints = Mathf.Max(healthPoints, regenHealthPts);
        }

        void Die()
        {
            if (isdead) return;
            GetComponent<Animator>().SetTrigger("Die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            isdead = true;
        }

        #region Interface
        public object CaptureState()
        {
            return healthPoints;
        }   //  ISaveable

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            if (healthPoints == 0f) { Die(); }
        }   //  ISaveable
        #endregion
    }
}