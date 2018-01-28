using UnityEngine;
using UnityEngine.UI;

public class Mana : MonoBehaviour {


    [SerializeField] RawImage manabar = null;
    [SerializeField] float maxMana = 100f;
    [SerializeField] float currentMana;
    CameraRaycaster camRaycaster;


    void Start()
    {
        currentMana = maxMana;
    }

    public bool IsManaAvailable(float amt)
    {
        return amt < currentMana;
    }

    public void ConsumeMana(float amt)
    {
        if (IsManaAvailable(amt))
        {
            float newMana = currentMana - amt;
            currentMana = Mathf.Clamp(newMana, 0, maxMana);
            //UpdateManaBar();
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