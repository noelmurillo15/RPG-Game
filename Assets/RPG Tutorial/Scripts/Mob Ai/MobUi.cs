// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;
using UnityEngine.UI;


namespace RPG {

    public class MobUi : MonoBehaviour {


        RawImage healthBarRawImage = null;
        HealthSystem mob = null;
        [SerializeField] Transform player;



        void Start()
        {
            mob = GetComponentInParent<HealthSystem>();
            healthBarRawImage = GetComponent<RawImage>();
        }

        void Update()
        {
            float xValue = -(mob.GetHealthAsPercentage() / 2f) - 0.5f;
            healthBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }
    }
}