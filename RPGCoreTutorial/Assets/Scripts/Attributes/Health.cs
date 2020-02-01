using ANM.Core;
using ANM.Saving;
using ANM.Stats;
using UnityEngine;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace ANM.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        //  Unity Events
        [SerializeField] private TakeDamageEvent takeDamage;

        //  Allows Dynamic float parameter which will be used int takeDamage.Invoke(<dynamic float>)
        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
            
        }

        //  Regen
        [SerializeField] private float regenPercentage = 75;

        private LazyValue<float> _healthPoints;  //  LazyValue will make sure healthPoints are initialized right before we use the healthpoints value by passing in a function
        private bool _isDead = false;
        private static readonly int Die1 = Animator.StringToHash("Die");


        private void Awake()
        {
            _healthPoints = new LazyValue<float>(GetInitialHealth);  //  GetInitialHealth() will get called right before the healthPoints.value is used ~ LazyInitialization
        }

        private void Start()
        {
            _healthPoints.ForceInit();   //  If healthPoints has not been accessed before this point, we'll force the value to be initialized
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().OnLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().OnLevelUp -= RegenerateHealth;
        }

        public bool IsDead()
        {
            return _isDead;
        }

        public float GetPercentage()
        {
            return GetFraction() * 100f;
        }

        public float GetFraction()
        {
            return _healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.HEALTH);
        }

        public float GetHealthPts()
        {
            return _healthPoints.value;
        }

        public float GetMaxHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.HEALTH);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + " took damage : " + damage);
            _healthPoints.value = Mathf.Max(_healthPoints.value - damage, 0);
            if (_healthPoints.value == 0f)
            {
                AwardExperience(instigator);
                Die();
            }
            else { takeDamage.Invoke(damage); }   //  Unity Event
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.HEALTH);
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.EXPERIENCE));
        }

        private void RegenerateHealth()
        {
            var regenHealthPts = GetComponent<BaseStats>().GetStat(Stat.HEALTH) * regenPercentage / 100;
            _healthPoints.value = Mathf.Max(_healthPoints.value, regenHealthPts);
        }

        private void Die()
        {
            if (_isDead) return;
            GetComponent<Animator>().SetTrigger(Die1);
            GetComponent<ActionScheduler>().CancelCurrentAction();
            _isDead = true;
        }

        #region Interface
        public object CaptureState()
        {
            return _healthPoints.value;
        }   //  ISaveable

        public void RestoreState(object state)
        {
            _healthPoints.value = (float)state;
            if (_healthPoints.value == 0f) { Die(); }
        }   //  ISaveable
        #endregion
    }
}