using System;
using UnityEngine;
using UnityEngine.UI;

public class Mana : MonoBehaviour {


    [SerializeField] RawImage manabar = null;
    [SerializeField] float maxMana = 100f;
    [SerializeField] float currentMana;
    [SerializeField] float regenAmount = 1f;
    CameraRaycaster camRaycaster;


    void Start()
    {
        currentMana = maxMana;
    }

    void FixedUpdate()
    {
        if(currentMana < maxMana)
        {
            AddMana(regenAmount);
        }
    }

    private void AddMana(float regenAmount)
    {
        var manaToAdd = regenAmount * Time.fixedDeltaTime;
        currentMana = Mathf.Clamp(currentMana + manaToAdd, 0, maxMana);
        UpdateManaBar();
    }

    public bool IsManaAvailable(float amt)
    {
        return amt <= currentMana;
    }

    public void ConsumeMana(float amt)
    {
        if (IsManaAvailable(amt))
        {
            float newMana = currentMana - amt;
            currentMana = Mathf.Clamp(newMana, 0, maxMana);
            UpdateManaBar();
        }
    }

    void UpdateManaBar()
    {
        //  TODO : remove magic numbers
        float xValue = -(ManaPercentage() / 2f) - .5f;
        manabar.uvRect = new Rect(xValue, 0f, .5f, 1f);
    }

    public float ManaPercentage()
    {
        return currentMana / maxMana;
    }
}