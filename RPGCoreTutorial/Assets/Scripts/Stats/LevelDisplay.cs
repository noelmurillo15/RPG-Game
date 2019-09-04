using System;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        //  Cached Variables
        BaseStats baseStats;


        void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        void Update()
        {
            GetComponent<Text>().text = String.Format("{0}", baseStats.GetLevel().ToString());
        }
    }
}