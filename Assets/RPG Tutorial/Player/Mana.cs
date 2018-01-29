// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Mana : MonoBehaviour {


    [SerializeField] Image manaOrb;
    [SerializeField] float maxMana = 100f;
    [SerializeField] float currentMana;
    [SerializeField] float regenAmount = 1f;
    [SerializeField] float manaCriticalPercent = .1f;
    [SerializeField] bool manaRegenActive = false;
    CameraRaycaster camRaycaster;


    void Start()
    {
        currentMana = maxMana;
    }

    IEnumerator ManaRegen()
    {
        manaRegenActive = true;
        while (currentMana < maxMana)
        {
            Regeneration(regenAmount);
            yield return new WaitForSeconds(.2f);
        }
        manaRegenActive = false;
    }

    public float ManaPercentage()
    {
        return currentMana / maxMana;

    }
    public bool IsManaAvailable(float amt)
    {
        return amt <= currentMana;
    }

    private void Regeneration(float hpToRegain)
    {
        AdjustMana(hpToRegain);
    }

    public void AdjustMana(float amt)
    {
        if (!IsManaAvailable(amt))
        {
            return;
        }

        currentMana = Mathf.Clamp(currentMana + amt, 0, maxMana);
        if (ManaPercentage() < manaCriticalPercent && !manaRegenActive)
        {
            StartCoroutine(ManaRegen());
        }
        UpdateManaBar();
    }

    void UpdateManaBar()
    {
        manaOrb.fillAmount = ManaPercentage();
    }    
}