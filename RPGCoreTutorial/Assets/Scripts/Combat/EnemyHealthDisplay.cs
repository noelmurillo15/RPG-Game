using System;
using UnityEngine;
using RPG.Resources;
using UnityEngine.UI;


namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;


        void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        void Update()
        {
            if (fighter.GetTarget() == null)
            {
                GetComponent<Text>().text = "N/A";
            }
            else
            {
                Health health = fighter.GetTarget();
                GetComponent<Text>().text = String.Format("{0:0}%", health.GetPercentage().ToString());
            }
        }
    }
}