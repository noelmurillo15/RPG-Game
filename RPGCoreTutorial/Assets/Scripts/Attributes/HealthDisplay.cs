/*
 * HealthDisplay - 
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
        
        //    TODO : If i have a higher max health when transitioning scenes,
        //    - the max health will reset back to 50 when new scene loads

        private void Awake()
        {
            _health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            GetComponent<TMP_Text>().text = $"{_health.GetHealthPts():0} / {_health.GetMaxHealth():0}";
        }
    }
}