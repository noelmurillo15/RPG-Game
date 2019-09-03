
using UnityEngine;
using System.Collections;


namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 5f;


        void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                StartCoroutine(HideForSeconds(respawnTime));
            }
        }

        IEnumerator HideForSeconds(float _seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(_seconds);
            ShowPickup(true);
        }

        void ShowPickup(bool _shouldShow)
        {
            GetComponent<SphereCollider>().enabled = _shouldShow;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(_shouldShow);
            }
        }
    }
}