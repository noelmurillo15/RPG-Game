using UnityEngine;
using System.Collections;
using RPG.Control;


namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 5f;


        void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                PickUp(other.GetComponent<Fighter>());
            }
        }

        void PickUp(Fighter fighter)
        {
            fighter.EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        void ShowPickup(bool _shouldShow)
        {
            GetComponent<SphereCollider>().enabled = _shouldShow;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(_shouldShow);
            }
        }

        IEnumerator HideForSeconds(float _seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(_seconds);
            ShowPickup(true);
        }

        #region Interface 
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