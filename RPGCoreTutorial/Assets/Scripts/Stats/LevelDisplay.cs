using TMPro;
using UnityEngine;

namespace ANM.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        //  Cached Variables
        private BaseStats _baseStats;


        private void Awake()
        {
            _baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            GetComponent<TMP_Text>().text = $"{_baseStats.GetLevel().ToString()}";
        }
    }
}