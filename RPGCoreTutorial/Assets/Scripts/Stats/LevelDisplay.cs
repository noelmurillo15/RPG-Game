using System;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        //  Cached Variables
        BaseStats baseStats;


        private void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            GetComponent<Text>().text = $"{baseStats.GetLevel().ToString()}";
        }
    }
}