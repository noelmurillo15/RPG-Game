/// <summary>
/// 2/13/18
/// Allan Murillo
/// RPG Core Project
/// MobUi.cs
/// </summary>
using UnityEngine;
using UnityEngine.UI;


namespace RPG {

    public class MobUi : MonoBehaviour {


        RawImage healthBarRawImage = null;
        HealthSystem mob = null;



        void Start()
        {
            mob = GetComponentInParent<HealthSystem>();
            healthBarRawImage = GetComponent<RawImage>();
        }

        void LateUpdate()
        {
            float xValue = -(mob.GetHealthAsPercentage() / 2f) - 0.5f;
            healthBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }
    }
}