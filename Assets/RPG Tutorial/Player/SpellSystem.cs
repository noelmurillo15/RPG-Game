// Allan Murillo : Unity RPG Core Test Project
using RPG;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class SpellSystem : MonoBehaviour {


    #region Properties
    //  Mana System
    [SerializeField] Image manaOrb;
    [SerializeField] float currentMana;
    [SerializeField] float maxMana = 100f;
    [SerializeField] float manaPerSec = 1f;
    [SerializeField] float regenTimer = .1f;
    [SerializeField] bool manaRegenActive = false;
    [SerializeField] float manaCriticalPercent = .1f;
    [SerializeField] AudioClip outOfMana;
    [SerializeField] AudioSource audioSource;

    //  Temporarily Serialized for debugging
    [SerializeField] SpellConfig[] spells;
    #endregion



    void Start()
    {
        currentMana = maxMana;
        AttachInitialSpells();
    }

    #region Mana System
    float EnergyAsPercent { get { return currentMana / maxMana; } }

    void UpdateManaBar()
    {
        manaOrb.fillAmount = EnergyAsPercent;
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

    public SpellConfig[] GetSpellList() { return spells; }

    public bool IsManaAvailable(float amt)
    {
        return amt <= currentMana;
    }

    public void LoseMana(float amt)
    {
        currentMana = Mathf.Clamp(currentMana - amt, 0f, maxMana);
        if (EnergyAsPercent < manaCriticalPercent)
        {
            if (audioSource && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(outOfMana);
            }
            StartCoroutine(ManaRegen());
        }
        UpdateManaBar();
    }

    public void GainMana(float amt)
    {
        currentMana = Mathf.Clamp(currentMana + amt, 0f, maxMana);
        UpdateManaBar();
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