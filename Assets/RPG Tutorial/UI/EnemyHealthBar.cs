// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;
using UnityEngine.UI;


public class EnemyHealthBar : MonoBehaviour
{
    RawImage healthBarRawImage = null;
    HealthSystem enemy = null;

    // Use this for initialization
    void Start()
    {
        enemy = GetComponentInParent<HealthSystem>();
        healthBarRawImage = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        float xValue = -(enemy.GetHealthAsPercentage() / 2f) - 0.5f;
        healthBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
    }
}