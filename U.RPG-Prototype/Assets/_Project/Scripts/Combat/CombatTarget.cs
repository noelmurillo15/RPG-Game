﻿/*
 * CombatTarget -
 * Created by : Allan N. Murillo
 * Last Edited : 2/25/2020
 */

using ANM.Control;
using UnityEngine;
using ANM.Attributes;
using ANM.Input;
using ANM.Scriptables.Manager;

namespace ANM.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        private InputController _inputController;


        private void Start()
        {
            _inputController = ResourcesManager.FindResource<InputController>("PlayerControls");
        }

        public CursorType GetCursorType()
        {
            return CursorType.COMBAT;
        }    //    IRaycastable

        public bool HandleRayCast(PlayerController controller)
        {
            if (!Fighter.CanAttack(gameObject)) return false;
            if (_inputController.IsPressed()) { controller.GetComponent<Fighter>().Attack(gameObject); }
            return true;
        }    //    IRaycastable
    }
}
