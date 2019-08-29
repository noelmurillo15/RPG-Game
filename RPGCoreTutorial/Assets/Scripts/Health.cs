using RPG.Saving;
using UnityEngine;


namespace RPG.Core
{    
    public class Health : MonoBehaviour, ISaveable {

        [SerializeField] float healthPoints = 100f;
        
        bool isdead = false;
        

        public bool IsDead(){
            return isdead;
        }
        
        public void TakeDamage(float _damage){
            healthPoints = Mathf.Max(healthPoints - _damage, 0);
            if(healthPoints == 0f)
            {
                Die();
            }
        }

        void Die()
        {
            if(isdead) return;
            GetComponent<Animator>().SetTrigger("Die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            isdead = true;
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            if(healthPoints == 0) { Die(); }
        }
    }
}