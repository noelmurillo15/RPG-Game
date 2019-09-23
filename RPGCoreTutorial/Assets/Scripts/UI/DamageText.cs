using TMPro;
using System;
using UnityEngine;


namespace RPG.UI
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] TMP_Text damageText;


        public void SetValue(float _amount)
        {
            damageText.text = String.Format("{0:0}", _amount);
        }

        public void DestroyText()
        {
            Destroy(gameObject);
        }
    }
}