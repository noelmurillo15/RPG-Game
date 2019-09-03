using RPG.Core;
using RPG.Stats;
using RPG.Saving;
using UnityEngine;


namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 100f;

        bool isdead = false;


        void Start()
        {
            healthPoints = GetComponent<BaseStats>().GetHealth();
        }

        public bool IsDead()
        {
            return isdead;
        }

        public void TakeDamage(float _damage)
        {
            healthPoints = Mathf.Max(healthPoints - _damage, 0);
            CheckIfDead();
        }

        public float GetPercentage(){            
            return (healthPoints / GetComponent<BaseStats>().GetHealth()) * 100f;
        }

        void Die()
        {
            if (isdead) return;
            GetComponent<Animator>().SetTrigger("Die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            isdead = true;
        }

        void CheckIfDead()
        {
            if (healthPoints == 0f) { Die(); }
        }

        #region Interface
        public object CaptureState()
        {
            return healthPoints;
        }   //  ISaveable

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            CheckIfDead();
        }   //  ISaveable
        #endregion
    }
}