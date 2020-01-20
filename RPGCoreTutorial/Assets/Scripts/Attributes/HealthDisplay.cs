using System;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        //  Cached Variables
        Health health;


        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            GetComponent<Text>().text = $"{health.GetHealthPts():0} / {health.GetMaxHealth():0}";
        }
    }
}