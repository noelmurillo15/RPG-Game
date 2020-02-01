using TMPro;
using UnityEngine;

namespace ANM.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        //  Cached Variables
        private Health _health;


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