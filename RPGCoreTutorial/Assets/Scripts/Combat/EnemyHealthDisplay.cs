using System;
using UnityEngine;
using RPG.Attributes;
using UnityEngine.UI;


namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        //  Cached Variables
        Fighter fighter;


        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            if (fighter.GetTarget() == null)
            {
                GetComponent<Text>().text = "N/A";
            }
            else
            {
                Health health = fighter.GetTarget();
                GetComponent<Text>().text = $"{health.GetPercentage().ToString():0}%";
            }
        }
    }
}