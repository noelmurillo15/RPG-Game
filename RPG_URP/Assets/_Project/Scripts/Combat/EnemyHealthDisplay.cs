/*
 * EnemyHealthDisplay - 
 * Created by : Allan N. Murillo
 * Last Edited : 2/25/2020
 */

using TMPro;
using UnityEngine;
using ANM.Attributes;

namespace ANM.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Fighter _fighter;
        private TMP_Text _text;


        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void FixedUpdate()
        {
            Health health = _fighter.GetTarget();
            _text.text = health == null ? "N/A" : $"{health.GetPercentage():0}%";
        }
    }
}