using System;
using System.Globalization;
using UnityEngine;
using RPG.Attributes;
using UnityEngine.UI;


namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        //  Cached Variables
        private Fighter _fighter;


        private void Awake()
        {
            _fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            if (_fighter.GetTarget() == null)
            {
                GetComponent<Text>().text = "N/A";
            }
            else
            {
                Health health = _fighter.GetTarget();
                GetComponent<Text>().text = $"{health.GetPercentage().ToString(CultureInfo.CurrentCulture):0}%";
            }
        }
    }
}