using UnityEngine;


namespace RPG.Attributes
{    
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas rootCanvas = null;


        void Update()
        {
            float hpFraction = healthComponent.GetFraction();

            if(Mathf.Approximately(hpFraction, 0) || Mathf.Approximately(hpFraction, 1)){
                rootCanvas.enabled = false;
                return;
            }

            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(hpFraction, 1, 1);
        }
    }
}