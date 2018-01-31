// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class HealthSystem : MonoBehaviour {


    #region Properties
    //  Health System
    [SerializeField] float currentHP;
    [SerializeField] float maxHP = 100f;
    [SerializeField] float deathVanishSeconds;

    [SerializeField] Image healthOrb;
    [SerializeField] AudioClip[] damageSounds;
    [SerializeField] AudioClip[] deathSounds;

    //  Animations
    const string DEATH_TRIGGER = "Death";

    //  References
    Animator myAnimator;
    //  TODO Audio Source for Player
    //AudioSource audioSource;  
    CharacterMovement characterMove;
    #endregion



    void Start()
    {
        myAnimator = GetComponent<Animator>();
        //audioSource = GetComponent<AudioSource>();
        characterMove = GetComponent<CharacterMovement>();

        currentHP = maxHP;
    }

    #region Health System
    void UpdateHealthBar()
    {
        if (healthOrb)
        {
            healthOrb.fillAmount = GetHealthAsPercentage();
        }
    }

    public float GetHealthAsPercentage()
    {
        return currentHP / maxHP;
    }

    public void Heal(float amt)
    {
        currentHP = Mathf.Clamp(currentHP + amt, 0f, maxHP);
        UpdateHealthBar();
    }

    public void TakeDamage(float dmg)
    {
        bool characterDead = (currentHP - dmg <= 0f);
        currentHP = Mathf.Clamp(currentHP - dmg, 0f, maxHP);

        //  TODO : Audio Player Hit
        //var clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
        //audioSource.PlayOneShot(clip);

        UpdateHealthBar();

        if (characterDead)
        {
            StartCoroutine(KillCharacter());
        }
    }    
    #endregion

    #region Death
    IEnumerator KillCharacter()
    {
        StopAllCoroutines();
        characterMove.Kill();
        myAnimator.SetTrigger(DEATH_TRIGGER);
       
        var playerComponent = GetComponent<Player>();
        if (playerComponent && playerComponent.isActiveAndEnabled)  //  relying on lazy envaluation
        {
            //  TODO : Audio Player Death
            //audioSource.clip = deathSounds[Random.Range(0, deathSounds.Length)];
            //audioSource.Play();

            yield return new WaitForSecondsRealtime(/*audioSource.clip.length*/ 1f);    
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        else
        {
            Destroy(gameObject, deathVanishSeconds);
        }
    }
    #endregion
}