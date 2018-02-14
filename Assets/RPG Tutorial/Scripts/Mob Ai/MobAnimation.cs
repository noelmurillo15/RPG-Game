/// <summary>
/// 2/13/18
/// Allan Murillo
/// RPG Core Project
/// MobAnimation.cs
/// </summary>
using UnityEngine;


namespace RPG {

    public class MobAnimation : MonoBehaviour {


        Character mobMaster;



        void Initialize()
        {
            mobMaster = GetComponent<Character>();
        }

        void OnEnable()
        {
            Initialize();
            mobMaster.EventCharacterDie += DisableAnimator;
            mobMaster.EventCharacterWalking += SetAnimationWalk;
            mobMaster.EventCharacterReachedNavTarget += SetAnimationIdle;
            mobMaster.EventCharacterAttack += SetAnimationAttack;
            mobMaster.EventCharacterTakeDamage += SetAnimationGetHit;
        }

        void OnDisable()
        {
            mobMaster.EventCharacterDie -= DisableAnimator;
            mobMaster.EventCharacterWalking -= SetAnimationWalk;
            mobMaster.EventCharacterReachedNavTarget -= SetAnimationIdle;
            mobMaster.EventCharacterAttack -= SetAnimationAttack;
            mobMaster.EventCharacterTakeDamage -= SetAnimationGetHit;
        }

        #region Animations
        void SetAnimationIdle()
        {
            if (mobMaster.MyAnim != null)
            {
                if (mobMaster.MyAnim.enabled)
                {
                    mobMaster.MyAnim.SetBool("isPursuing", false);
                }
            }
        }

        void SetAnimationWalk()
        {
            if (mobMaster.MyAnim != null)
            {
                if (mobMaster.MyAnim.enabled)
                {
                    mobMaster.MyAnim.SetBool("isPursuing", true);
                }
            }
        }

        void SetAnimationAttack()
        {
            if (mobMaster.MyAnim != null)
            {
                if (mobMaster.MyAnim.enabled)
                {
                    mobMaster.MyAnim.SetTrigger("Attack");
                }
            }
        }

        void SetAnimationGetHit(float dummy)
        {
            if (mobMaster.MyAnim != null)
            {
                if (mobMaster.MyAnim.enabled && !mobMaster.IsCriticallyHit)
                {
                    mobMaster.IsCriticallyHit = true;
                    mobMaster.MyAnim.SetTrigger("GetHit");
                }
            }
        }
        /// <summary>
        /// GetHit Animation MUST call this function
        /// through the animation event sytem
        /// </summary>
        void OnMobGetHit()
        {
            mobMaster.IsCriticallyHit = false;
        }
        /// <summary>
        /// Disables Upon Death
        /// </summary>
        void DisableAnimator()
        {
            if (mobMaster.MyAnim != null)
            {
                mobMaster.MyAnim.enabled = false;
            }
        }
        #endregion
    }
}