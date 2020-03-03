/*
 * Health - Base Health for any Character
 * Implements ISaveable which is used by the save system
 * Created by : Allan N. Murillo
 * Last Edited : 2/25/2020
 */

using ANM.Core;
using ANM.Stats;
using ANM.Saving;
using UnityEngine;
using UnityEngine.AI;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace ANM.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private TakeDamageEvent takeDamage;
        //  Allows Dynamic float parameter which will be used int takeDamage.Invoke(<dynamic float>)
        [System.Serializable] public class TakeDamageEvent : UnityEvent<float> { }

        [SerializeField] private float regenPercentage = 100;

        private bool _isDead;
        private const float OneHundred = 100f;
        //  LazyValue will make sure floats are initialized right before we use the health points value by passing in a function
        private LazyValue<float> _healthPoints;  
        private LazyValue<float> _maxHealthPoints;
        private BaseStats _baseStats;
        private static readonly int Die1 = Animator.StringToHash("Die");


        private void Awake()
        {    //  GetInitialHealth() will get called right before the healthPoints.value is used ~ LazyInitialization
            _healthPoints = new LazyValue<float>(GetInitialHealth);  
            _maxHealthPoints = new LazyValue<float>(GetInitialHealth);
            _baseStats = GetComponent<BaseStats>();
        }    //    Needed for saving / loading game state so that these values can be accessed before Start()

        private void Start()
        {    //  If healthPoints has not been accessed before this point, we'll force the value to be initialized
            _healthPoints.ForceInit();   
            _maxHealthPoints.ForceInit();
        }

        private void OnEnable()
        {
            _baseStats.LevelUpEvent += RegenerateHealth;
        }

        private void OnDisable()
        {
            _baseStats.LevelUpEvent -= RegenerateHealth;
        }
        
        private void Die()
        {
            if (_isDead) return;
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<Animator>().SetTrigger(Die1);
            GetComponent<NavMeshAgent>().enabled = false;
            var rigid = GetComponent<Rigidbody>();
            rigid.velocity = Vector3.zero;
            rigid.mass = 1000;
            _isDead = true;
        }
        
        private float GetInitialHealth()
        {
            return _baseStats.GetStat(Stat.HEALTH);
        }
        
        private void RegenerateHealth()
        {
            var regenHealthPts = GetMaxHealth() * regenPercentage / OneHundred;
            _healthPoints.value = Mathf.Max(_healthPoints.value, regenHealthPts);
        }

        private void AwardExperience(GameObject instigator)
        {
            var experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainExperience(_baseStats.GetStat(Stat.EXP_TO_GIVE));
        }
        
        public bool IsDead() { return _isDead; }

        public float GetPercentage() { return GetFraction() * OneHundred; }

        public float GetFraction() { return GetHealthPts() / GetMaxHealth(); }

        public float GetHealthPts() { return _healthPoints.value; }

        public float GetMaxHealth()
        {
            _maxHealthPoints.value = _baseStats.GetStat(Stat.HEALTH);
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