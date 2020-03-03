/*
 * HealthDisplay - Displays a characters Health & Max Health to a TMP Text
 * Created by : Allan N. Murillo
 * Last Edited : 2/25/2020
 */

using TMPro;
using UnityEngine;

namespace ANM.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        private Health _health;
        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            _text.text = $"{_health.GetHealthPts():0} / {_health.GetMaxHealth():0}";
        }
    }
}