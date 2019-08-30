using UnityEngine;


namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon = null;


        void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                Destroy(gameObject);
            }
        }
    }
}