/// <summary>
/// 2/13/18
/// Allan Murillo
/// RPG Core Project
/// SpellSystem.cs
/// </summary>
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace RPG {

    public class SpellSystem : MonoBehaviour {


        #region Properties
        [Header("Mana")]
        [SerializeField] float currentMana = 0f;
        [SerializeField] float maxMana = 100f;
        [SerializeField] float manaCriticalPercent = .1f;
        [SerializeField] float manaPerSec = 1f;
        [SerializeField] float regenTimer = .1f;
        [SerializeField] bool manaRegenActive = false;
        [SerializeField] Image manaOrb;
        [SerializeField] AudioClip outOfMana;
        [SerializeField] AudioSource audioSource;

        [Header("Spells")]
        [SerializeField] SpellConfig[] spells;
        #endregion



        void Start()
        {
            currentMana = maxMana;
            AttachInitialSpells();
        }

        #region Accessors
        public SpellConfig[] GetSpellList() { return spells; }
        public float ManaAsPercent { get { return currentMana / maxMana; } }
        #endregion

        #region Mana System
        void UpdateManaBar()
        {
            manaOrb.fillAmount = ManaAsPercent;
        }

        public bool IsManaAvailable(float mana)
        {
            return mana <= currentMana;
        }

        public void GainMana(float mana)
        {
            currentMana = Mathf.Clamp(currentMana + mana, 0f, maxMana);
            UpdateManaBar();
        }

        public void LoseMana(float mana)
        {
            currentMana = Mathf.Clamp(currentMana - mana, 0f, maxMana);
            if (ManaAsPercent < manaCriticalPercent)
            {
                if (audioSource && !audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(outOfMana);
                }
                StartCoroutine(ManaRegen());
            }
            UpdateManaBar();
        }

        IEnumerator ManaRegen()
        {
            manaRegenActive = true;
            while (currentMana < maxMana)
            {
                GainMana(manaPerSec);
                yield return new WaitForSeconds(regenTimer);
            }
            manaRegenActive = false;
        }
        #endregion

        #region Spell System
        void AttachInitialSpells()
        {
            for (int spellIndex = 0; spellIndex < spells.Length; spellIndex++)
            {
                spells[spellIndex].AttachSpell(gameObject);
            }
        }

        public void AttemptSpell(int spellIndex, GameObject target = null)
        {
            var mySpell = spells[spellIndex];
            if (IsManaAvailable(mySpell.GetManaCost()))
            {
                mySpell.Activate(target);
                LoseMana(mySpell.GetManaCost());
            }
        }
        #endregion
    }
}