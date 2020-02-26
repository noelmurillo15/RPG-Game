/*
 * WeaponPickup - 
 * Created by : Allan N. Murillo
 * Last Edited : 2/25/2020
 */

using ANM.Control;
using UnityEngine;
using System.Collections;

namespace ANM.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] private WeaponConfig weapon = null;
        [SerializeField] private float respawnTime = 5f;


        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                PickUp(other.GetComponent<Fighter>());
            }
        }

        private void PickUp(Fighter fighter)
        {
            fighter.EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<SphereCollider>().enabled = shouldShow;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        #region Interfaces
        public bool HandleRayCast(PlayerController controller)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PickUp(controller.GetComponent<Fighter>());
            }
            return true;
        }   //  IRaycastable

        public CursorType GetCursorType()
        {
            return CursorType.PICKUP;
        }   //  IRaycastable
        #endregion
    }
}