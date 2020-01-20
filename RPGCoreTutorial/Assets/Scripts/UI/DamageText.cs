using TMPro;
using System;
using UnityEngine;


namespace RPG.UI
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] TMP_Text damageText;


        public void SetValue(float amount)
        {
            damageText.text = $"{amount:0}";
        }

        public void DestroyText()
        {
            Destroy(gameObject);
        }
    }
}