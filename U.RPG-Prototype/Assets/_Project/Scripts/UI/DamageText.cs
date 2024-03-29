﻿/*
 * DamageText - 
 * Created by : Allan N. Murillo
 * Last Edited : 2/25/2020
 */

using TMPro;
using UnityEngine;

namespace ANM.UI
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private TMP_Text damageText;


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