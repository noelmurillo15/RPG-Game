/*
 * LevelDisplay - 
 * Created by : Allan N. Murillo
 * Last Edited : 2/25/2020
 */

using TMPro;
using UnityEngine;

namespace ANM.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        private BaseStats _baseStats;
        private TMP_Text _text;


        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            _text.text = $"{_baseStats.GetLevel().ToString()}";
        }
    }
}