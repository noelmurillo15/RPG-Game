using TMPro;
using UnityEngine;
using System.Globalization;

namespace ANM.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        //  Cached Variables
        private Experience _exp;


        private void Awake()
        {
            _exp = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            GetComponent<TMP_Text>().text = 
                $"{_exp.GetExperiencePts().ToString(CultureInfo.CurrentCulture)}pts";
        }
    }
}