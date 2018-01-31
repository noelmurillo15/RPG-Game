// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

    [RequireComponent(typeof(MobMaster))]
    [RequireComponent(typeof(Animator))]
    public class MobAnimation : MonoBehaviour {

        MobMaster mobMaster;
        Animator myAnimator;


        void Initialize()
        {
            mobMaster = GetComponent<MobMaster>();
            if (GetComponent<Animator>() != null)
            {
                myAnimator = GetComponent<Animator>();
            }
        }

        void OnEnable()
        {
            Initialize();
            mobMaster.EventMobDie += DisableAnimator;
            mobMaster.EventMobWalking += SetAnimationWalk;
            mobMaster.EventMobReachedNavTarget += SetAnimationIdle;
            mobMaster.EventMobAttack += SetAnimationAttack;
            mobMaster.EventMobDeductHealth += SetAnimationGetHit;
        }

        void OnDisable()
        {
            mobMaster.EventMobDie -= DisableAnimator;
            mobMaster.EventMobWalking -= SetAnimationWalk;
            mobMaster.EventMobReachedNavTarget -= SetAnimationIdle;
            mobMaster.EventMobAttack -= SetAnimationAttack;
            mobMaster.EventMobDeductHealth -= SetAnimationGetHit;
        }

        #region Animations
        void SetAnimationIdle()
        {
            if (myAnimator != null)
            {
                if (myAnimator.enabled)
                {
                    myAnimator.SetBool("isPursuing", false);
                }
            }
        }

        void SetAnimationWalk()
        {
            if (myAnimator != null)
            {
                if (myAnimator.enabled)
                {
                    myAnimator.SetBool("isPursuing", true);
                }
            }
        }

        void SetAnimationAttack()
        {
            if (myAnimator != null)
            {
                if (myAnimator.enabled)
                {
                    myAnimator.SetTrigger("Attack");
                }
            }
        }

        void SetAnimationGetHit(int num)
        {
            if (myAnimator != null)
            {
                if (myAnimator.enabled && !mobMaster.IsCriticallyHit)
                {
                    mobMaster.IsCriticallyHit = true;
                    myAnimator.SetTrigger("GetHit");
                }
            }
        }
        //  TODO : Gethit animation must have event to use this function
        void OnMobGetHit()
        {
            mobMaster.IsCriticallyHit = false;
        }

        void DisableAnimator()
        {
            if (myAnimator != null)
            {
                myAnimator.enabled = false;
            }
        }
        #endregion
    }
}