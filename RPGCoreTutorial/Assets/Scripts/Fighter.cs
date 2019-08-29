using RPG.Core;
using UnityEngine;
using RPG.Movement;


namespace RPG.Combat
{
    [RequireComponent(typeof(Animator))]
    public class Fighter : MonoBehaviour, IAction {
        #region Fighter Class Members
        Health target;
        Animator myAnimator;
        CharacterMove myCharacterMove;

        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 20f;
        float timeSinceLastAttack = Mathf.Infinity;
        #endregion


        void Start() {
            myAnimator = GetComponent<Animator>();
            myCharacterMove = GetComponent<CharacterMove>();
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if(target == null) return;
            if(target.IsDead()) return;

            if (!GetIsInRange())
            {
                myCharacterMove.MoveTo(target.transform.position);
            }
            else
            {
                myCharacterMove.Cancel();
                AttackBehaviour();
            }
        }

        public void Attack(GameObject _combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = _combatTarget.GetComponent<Health>();
        }

        public bool CanAttack(GameObject _combatTarget){
            if(_combatTarget == null) return false;
            Health targetToTest = _combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if(timeSinceLastAttack >= timeBetweenAttacks)
            {                
                TriggerAttack();
                timeSinceLastAttack = 0f;
            }
        }

        void TriggerAttack()
        {
            //  This will trigger the Animation Event : Hit()
            myAnimator.ResetTrigger("StopAttack");
            myAnimator.SetTrigger("Attack");
        }

        void StopAttack()
        {
            myAnimator.ResetTrigger("Attack");
            myAnimator.SetTrigger("StopAttack");
        }

        #region Animation Events
        void Hit()
        {
            if(target == null) return;
            target.TakeDamage(weaponDamage);
        }
        #endregion                

        #region Interface Methods
        public void Cancel()
        {
            StopAttack();
            target = null;
        }        
        #endregion
    }
}