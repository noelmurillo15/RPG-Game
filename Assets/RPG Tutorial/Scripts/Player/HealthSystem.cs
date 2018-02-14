/// <summary>
/// 2/13/18
/// Allan Murillo
/// RPG Core Project
/// HealthSystem.cs
/// </summary>
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace RPG {

    public class HealthSystem : MonoBehaviour {


        #region Properties
        [Header("Health")]
        [SerializeField] float currentHP;
        [SerializeField] float maxHP = 100f;
        [SerializeField] float deathVanishSeconds;

        [SerializeField] Image healthOrb;
        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;

        //  Animations
        const string DEATH_TRIGGER = "Death";

        //  References
        Character characterMaster;
        //  TODO Audio Source for Player
        //AudioSource audioSource;  
        #endregion


        void Initialize()
        {
            currentHP = maxHP;
            //audioSource = GetComponent<AudioSource>();
            characterMaster = GetComponent<Character>();
        }

        void OnEnable()
        {
            Initialize();
            characterMaster.EventCharacterHeal += Heal;
            characterMaster.EventCharacterTakeDamage += TakeDamage;
        }

        void OnDisable()
        {
            characterMaster.EventCharacterHeal -= Heal;
            characterMaster.EventCharacterTakeDamage -= TakeDamage;
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
            characterMaster.MyAnim.SetTrigger(DEATH_TRIGGER);

            var playerComponent = GetComponent<PlayerMaster>();
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
}