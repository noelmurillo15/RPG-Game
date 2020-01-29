using UnityEngine;


namespace RPG.Attributes
{    
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health healthComponent = null;
        [SerializeField] private RectTransform foreground = null;
        [SerializeField] private Canvas rootCanvas = null;


        private void Update()
        {
            var hpFraction = healthComponent.GetFraction();

            if(Mathf.Approximately(hpFraction, 0) || Mathf.Approximately(hpFraction, 1)){
                rootCanvas.enabled = false;
                return;
            }

            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(hpFraction, 1, 1);
        }
    }
}