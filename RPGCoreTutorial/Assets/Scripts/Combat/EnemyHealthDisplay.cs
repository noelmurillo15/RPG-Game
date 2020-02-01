using System.Globalization;
using ANM.Attributes;
using TMPro;
using UnityEngine;

namespace ANM.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        //  Cached Variables
        private Fighter _fighter;


        private void Awake()
        {
            _fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            if (_fighter.GetTarget() == null)
            {
                GetComponent<TMP_Text>().text = "N/A";
            }
            else
            {
                Health health = _fighter.GetTarget();
                GetComponent<TMP_Text>().text = $"{health.GetPercentage().ToString(CultureInfo.CurrentCulture):0}%";
            }
        }
    }
}