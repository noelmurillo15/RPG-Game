using RPG.Core;
using RPG.Stats;
using RPG.Saving;
using UnityEngine;
using GameDevTV.Utils;


namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenrationPercentage = 75;

        LazyValue<float> healthPoints;  //  LazyValue will make sure healthPoints are initialized right before we use the healthpoints value by passing in a function
        bool isdead = false;


        void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);  //  GetInitialHealth() will get called right before the healthPoints.value is used ~ LazyInitialization
        }

        void Start()
        {
            healthPoints.ForceInit();   //  If healthPoints has not been accessed before this point, we'll force the value to be initialized
        }

        void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        public bool IsDead()
        {
            return isdead;
        }

        public float GetPercentage()
        {
            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.HEALTH) * 100f;
        }

        public float GetHealthPts()
        {
            return healthPoints.value;
        }

        public float GetMaxHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.HEALTH);
        }

        public void TakeDamage(GameObject instigator, float _damage)
        {
            print(gameObject.name + " took damage : " + _damage);
            healthPoints.value = Mathf.Max(healthPoints.value - _damage, 0);
            if (healthPoints.value == 0f)
            {
                AwardExperience(instigator);
                Die();
            }
        }

        float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.HEALTH);
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
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPts);
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
            return healthPoints.value;
        }   //  ISaveable

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;
            if (healthPoints.value == 0f) { Die(); }
        }   //  ISaveable
        #endregion
    }
}