// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;
using UnityEngine.UI;


namespace RPG {

    public class MobUi : MonoBehaviour {


        RawImage healthBarRawImage = null;
        MobMaster mob = null;



        void Start()
        {
            mob = GetComponentInParent<MobMaster>();
            healthBarRawImage = GetComponent<RawImage>();
        }

        void Update()
        {
            float xValue = -(mob.MobHealthSystem.GetHealthAsPercentage() / 2f) - 0.5f;
            healthBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }
    }
}