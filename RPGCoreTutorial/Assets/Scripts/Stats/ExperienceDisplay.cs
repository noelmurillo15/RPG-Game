using System;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        //  Cached Variables
        Experience exp;


        private void Awake()
        {
            exp = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            GetComponent<Text>().text = $"{exp.GetExperiencePts().ToString()}pts";
        }
    }
}