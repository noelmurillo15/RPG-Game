/*
 * SpellSystem -
 * Created by : Allan N. Murillo
 * Last Edited : 3/24/2020
 */

using UnityEngine;
using UnityEngine.UI;
using RPG.SpellSystem;
using System.Collections;
using ANM.FPS.Spells;
//using UnityEngine.InputSystem;

namespace ANM.SpellSystem
{
    public class SpellSystem : MonoBehaviour
    {
        [SerializeField] private Image manaOrb = null;
        [SerializeField] private float currentMana = 0f;
        [SerializeField] private float maxMana = 100f;
        [SerializeField] private float manaPerSec = 1f;
        [SerializeField] private float regenTimer = .1f;
        [SerializeField] private bool manaRegenActive = false;
        [SerializeField] private float manaCriticalPercent = .1f;
        [SerializeField] private AudioClip outOfMana = null;
        [SerializeField] private AudioSource audioSource = null;

        [SerializeField] private SpellConfig[] myActiveSpells = null;
        [SerializeField] private Image[] spellIcons_HUD = null;
        [SerializeField] private SpellMaster spellMaster = null;

        //private InputAction _spellAction;

        //    TODO : create movement altering spells (ex:jump boost, forward momentum boost)

        //    TODO : create 4 magic types - each type allows a few spells but magic is lost when below a certain hp

        //    TODO : after using a magic type, the magic can run out and force a different magic out unless chosen

        private void Awake()
        {
            //  TIP : Creating free-standing actions only works if on the same gameobject as PlayerInput Component
            /*_spellAction = new InputAction("Spells", binding: "<Keyboard>/1");
            _spellAction.AddBinding("<Keyboard>/2");
            _spellAction.AddBinding("<Keyboard>/3");
            _spellAction.AddBinding("<Keyboard>/4");
            _spellAction.AddBinding("<Keyboard>/5");
            _spellAction.AddBinding("<Keyboard>/6");
            _spellAction.AddBinding("<Keyboard>/7");
            _spellAction.AddBinding("<Keyboard>/8");
            _spellAction.AddBinding("<Keyboard>/9");
            _spellAction.Enable();
            _spellAction.performed += SpellInput;*/
        }

        private void Start()
        {
            manaRegenActive = false;
            currentMana = maxMana;
            UpdateSpellBook();
        }

        private void OnDisable()
        {
            //  TIP : Disable free-standing input actions
            /*_spellAction.performed -= SpellInput;
            _spellAction.Disable();*/
        }

        #region Mana System

        private float ManaAsPercent => currentMana / maxMana;

        private void UpdateManaBar()
        {
            manaOrb.fillAmount = ManaAsPercent;
        }

        private IEnumerator ManaRegen()
        {
            manaRegenActive = true;
            while (currentMana < maxMana)
            {
                GainMana(manaPerSec);
                yield return new WaitForSeconds(regenTimer);
            }

            manaRegenActive = false;
        }

        public SpellConfig[] GetActiveSpellList()
        {
            return myActiveSpells;
        }

        private bool IsManaAvailable(float amt)
        {
            var result = amt <= currentMana;
            if (result)
            {
                LoseMana(amt);
            }

            return result;
        }

        private void LoseMana(float amt)
        {
            currentMana = Mathf.Clamp(currentMana - amt, 0f, maxMana);
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

        private void GainMana(float amt)
        {
            currentMana = Mathf.Clamp(currentMana + amt, 0f, maxMana);
            UpdateManaBar();
        }

        #endregion

        #region Spell System

        private void UpdateSpellBook()
        {
            for (var spellIndex = 0; spellIndex < myActiveSpells.Length; spellIndex++)
            {
                if (myActiveSpells[spellIndex] == null)
                {
                    spellIcons_HUD[spellIndex].sprite = spellMaster.emptySpell.GetSpellIcon();
                    continue;
                }

                myActiveSpells[spellIndex].AttachSpell(gameObject);
                spellIcons_HUD[spellIndex].sprite = myActiveSpells[spellIndex].GetSpellIcon();
            }
        }

        private void ParseSpellInput(string keypress)
        {
            if (int.TryParse(keypress, out var keyIndex))
            {
                ActivateSpell(keyIndex - 1);
            }
        }

        public void ActivateSpell(int spellIndex)
        {
            if (myActiveSpells.Length - 1 < spellIndex) return;

            var mySpell = myActiveSpells[spellIndex];

            if (mySpell == null)
            {
                return;
            }

            if (IsManaAvailable(mySpell.GetManaCost()))
            {
                mySpell.Activate();
            }
        }

        #endregion

        #region Detect Input

        /*private void SpellInput(InputAction.CallbackContext context)
        {
            ParseSpellInput(context.control.name);
        }*/

        #endregion
    }
}
