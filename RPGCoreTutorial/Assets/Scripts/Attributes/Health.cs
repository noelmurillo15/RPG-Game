/*
 * Health - 
 * Created by : Allan N. Murillo
 * Last Edited : 2/25/2020
 */

using ANM.Core;
using ANM.Stats;
using ANM.Saving;
using UnityEngine;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace ANM.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        //  Unity Events
        [SerializeField] private TakeDamageEvent takeDamage;
        [System.Serializable] public class TakeDamageEvent : UnityEvent<float> { }
        //  Allows Dynamic float parameter which will be used int takeDamage.Invoke(<dynamic float>)

        [SerializeField] private float regenPercentage = 100;

        private bool _isDead;
        private LazyValue<float> _healthPoints;  //  LazyValue will make sure healthPoints are initialized right before we use the health points value by passing in a function
        private LazyValue<float> _maxHealthPoints;
        private static readonly int Die1 = Animator.StringToHash("Die");


        private void Awake()
        {
            _healthPoints = new LazyValue<float>(GetInitialHealth);  //  GetInitialHealth() will get called right before the healthPoints.value is used ~ LazyInitialization
            _maxHealthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private void Start()
        {
            _healthPoints.ForceInit();   //  If healthPoints has not been accessed before this point, we'll force the value to be initialized
            _maxHealthPoints.ForceInit();
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().LevelUpEvent += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().LevelUpEvent -= RegenerateHealth;
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
            return GetHealthPts() / GetMaxHealth();
        }

        public float GetHealthPts()
        {
            return _healthPoints.value;
        }

        public float GetMaxHealth()
        {
            _maxHealthPoints.value = GetComponent<BaseStats>().GetStat(Stat.HEALTH);
            return _maxHealthPoints.value;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            _healthPoints.value = Mathf.Max(_healthPoints.value - damage, 0);
            if (_healthPoints.value <= 0f)
            {
                AwardExperience(instigator);
                Die();
            }
            else takeDamage.Invoke(damage); 
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.HEALTH);
        }

        private void AwardExperience(GameObject instigator)
        {
            var experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.EXPERIENCE));
        }

        private void RegenerateHealth()
        {
            var regenHealthPts = GetMaxHealth() * regenPercentage / 100;
            _healthPoints.value = Mathf.Max(_healthPoints.value, regenHealthPts);
        }

        private void Die()
        {
            if (_isDead) return;
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Animator>().SetTrigger(Die1);
            var rigid = GetComponent<Rigidbody>();
            rigid.isKinematic = true;
            rigid.useGravity = true;
            rigid.mass = 1000;
            _isDead = true;
        }

        #region Interface
        public object CaptureState()
        {
            var healthProperty = new HealthProperty(GetHealthPts(), GetMaxHealth());
            return healthProperty;
        }   //  ISaveable

        public void RestoreState(object state)
        {
            _healthPoints.value = ((HealthProperty)state).curHealth;
            _maxHealthPoints.value = ((HealthProperty)state).maxHealth;
            if (_healthPoints.value <= 0f) { Die(); }
        }   //  ISaveable
        #endregion
    }
    
    [System.Serializable]
    internal struct HealthProperty
    {
        public float curHealth;
        public float maxHealth;

        public HealthProperty(float x, float y)
        {
            curHealth = x;
            maxHealth = y;
        }
    }
}