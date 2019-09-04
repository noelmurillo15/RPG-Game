﻿using System;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience exp;


        void Awake()
        {
            exp = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        void Update()
        {
            GetComponent<Text>().text = String.Format("{0}pts", exp.GetExperiencePts().ToString());
        }
    }
}