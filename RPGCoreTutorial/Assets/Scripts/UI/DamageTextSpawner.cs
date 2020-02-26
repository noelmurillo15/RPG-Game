/*
 * DamageTextSpawner - 
 * Created by : Allan N. Murillo
 * Last Edited : 2/25/2020
 */

using ANM.Framework.Extensions;
using UnityEngine;

namespace ANM.UI
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText damageTextPrefab = null;


        public void Spawn(float damage)
        {
            DamageText instance = Instantiate(damageTextPrefab, transform);
            instance.SetValue(damage);
            instance.transform.position.With(y: instance.transform.position.y + 3);
        }
    }
}