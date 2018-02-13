///<summary>
///
/// </summary>
using UnityEngine;


public class Character : MonoBehaviour {


    [SerializeField] Transform myAttackTarget;

    #region Events
    public delegate void GeneralEventHandler();
    public delegate void StatsEventHandler(float hp);
    public delegate void NavTargetEventHandler(Transform targetTransform);

    public event GeneralEventHandler EventCharacterDie;
    public event GeneralEventHandler EventCharacterAttack;
    public event GeneralEventHandler EventCharacterWalking;
    public event GeneralEventHandler EventCharacterLostTarget;
    public event GeneralEventHandler EventCharacterReachedNavTarget;

    public event StatsEventHandler EventCharacterHeal;
    public event StatsEventHandler EventCharacterTakeDamage;
    public event StatsEventHandler EventCharacterGainExperience;

    public event NavTargetEventHandler EventSetCharacterNavTarget;
    #endregion



    public Transform AttackTarget { get { return myAttackTarget; } set { myAttackTarget = value; } }

    #region Event Methods
    /// <summary>
    /// 
    /// </summary>
    public void CallEventCharacterDie()
    {
        if (EventCharacterDie != null)
        {
            EventCharacterDie();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void CallEventCharacterAttack()
    {
        if (EventCharacterAttack != null)
        {
            EventCharacterAttack();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void CallEventCharacterWalking()
    {
        if (EventCharacterWalking != null)
        {
            EventCharacterWalking();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void CallEventCharacterLostTarget()
    {
        if (EventCharacterLostTarget != null)
        {
            EventCharacterLostTarget();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void CallEventCharacterReachedNavTarget()
    {
        if (EventCharacterReachedNavTarget != null)
        {
            EventCharacterReachedNavTarget();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="hp"></param>
    public void CallEventCharacterHeal(float hp)
    {
        if (EventCharacterHeal != null)
        {            
            EventCharacterHeal(hp);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="hp"></param>
    public void CallEventCharacterTakeDamage(float hp)
    {
        if (EventCharacterTakeDamage != null)
        {
            EventCharacterTakeDamage(hp);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="exp"></param>
    public void CallEventCharacterGainExperience(float exp)
    {
        if (EventCharacterGainExperience != null)
        {
            EventCharacterGainExperience(exp);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetTransform"></param>
    public void CallEventSetCharacterNavTarget(Transform targetTransform)
    {
        if (EventSetCharacterNavTarget != null)
        {
            EventSetCharacterNavTarget(targetTransform);
            AttackTarget = targetTransform;
        }
    }
    #endregion
}