/*
 * SpellMaster -
 * Created by : Allan N. Murillo
 * Last Edited : 4/7/2020
 */

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RPG.SpellSystem;
//using ANM.Game_Management;
//using UnityEngine.InputSystem;

namespace ANM.FPS.Spells
{
    public class SpellMaster : MonoBehaviour
    {
        [SerializeField] private GameObject spellListPanel = null;
        [SerializeField] private GameObject projectileSpellContent = null;
        [SerializeField] private GameObject aoeSpellContent = null;
        [SerializeField] private GameObject buffSpellContent = null;
        [SerializeField] private GameObject spellHudItemPrefab = null;

        public SpellConfig[] projectileSpellList = null;
        public SpellConfig[] aoeSpellList = null;
        public SpellConfig[] buffSpellList = null;
        public SpellConfig emptySpell = null;

        //private GameToggleCursor _cursorToggle;
        //private Keyboard _myKeyboard;
        private float _inputBuffer;
        private bool _populated;


        private void Start()
        {
            _inputBuffer = 0f;
            //_myKeyboard = Keyboard.current;
            //_cursorToggle = FindObjectOfType<GameToggleCursor>();

            if (!HasBeenPopulated())
            {
                PopulateSpells();
            }

            if (spellListPanel != null)
            {
                spellListPanel.SetActive(false);
            }
        }

        private void Update()
        {
            //if (!_myKeyboard.kKey.isPressed || !(Time.time > _inputBuffer)) return;
            if (spellListPanel == null) return;
            _inputBuffer = Time.time + 0.2f;
            spellListPanel.SetActive(!spellListPanel.activeSelf);
            //_cursorToggle.SetCursorLocked(!spellListPanel.activeSelf);
        }

        private bool HasBeenPopulated()
        {
            return _populated;
        }

        private void PopulateSpells()
        {
            foreach (var uniqueSpell in projectileSpellList)
            {
                var projectileSpellItem = Instantiate(spellHudItemPrefab, projectileSpellContent.transform, false);
                var spellIcon = projectileSpellItem.GetComponentInChildren<Image>();
                spellIcon.sprite = uniqueSpell.GetSpellIcon();
                var spellName = projectileSpellItem.GetComponentInChildren<TMP_Text>();
                spellName.text = uniqueSpell.name;
            }

            foreach (var uniqueSpell in aoeSpellList)
            {
                var aoeSpellItem = Instantiate(spellHudItemPrefab, aoeSpellContent.transform, false);
                var spellIcon = aoeSpellItem.GetComponentInChildren<Image>();
                spellIcon.sprite = uniqueSpell.GetSpellIcon();
                var spellName = aoeSpellItem.GetComponentInChildren<TMP_Text>();
                spellName.text = uniqueSpell.name;
            }

            foreach (var uniqueSpell in buffSpellList)
            {
                var buffSpellItem = Instantiate(spellHudItemPrefab, buffSpellContent.transform, false);
                var spellIcon = buffSpellItem.GetComponentInChildren<Image>();
                spellIcon.sprite = uniqueSpell.GetSpellIcon();
                var spellName = buffSpellItem.GetComponentInChildren<TMP_Text>();
                spellName.text = uniqueSpell.name;
            }

            _populated = true;
        }
    }
}
