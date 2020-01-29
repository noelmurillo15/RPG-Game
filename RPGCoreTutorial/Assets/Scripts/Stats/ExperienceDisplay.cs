using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        //  Cached Variables
        private Experience _exp;


        private void Awake()
        {
            _exp = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            GetComponent<Text>().text = $"{_exp.GetExperiencePts().ToString(CultureInfo.CurrentCulture)}pts";
        }
    }
}