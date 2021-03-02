/*
 * SpellUI -
 * Created by : Allan N. Murillo
 * Last Edited : 3/26/2020
 */

using ANM.Control;
using UnityEngine;
using RPG.SpellSystem;
//using UnityEngine.InputSystem;

namespace ANM.SpellSystem
{
    public class SpellUI : MonoBehaviour
    {
        [SerializeField] private GameObject radialSpellUi = null;
        [SerializeField] private LayerMask spellTargetable = 0;

        private Camera _mainCamera;

        private readonly Vector3 _screenCenter = new Vector3(0.5f, 0.5f, 0f);
        private SpellBehaviour _spellBehaviour;
        private const float MaxDistance = 25f;
        private RaycastHit _hitInfo;
        private bool _canCast;
        public bool casting;
        //private Mouse _myMouse;


        private void Start()
        {
            var playerMaster = GetComponent<PlayerController>();
            _mainCamera = Camera.main;
            SpellUiDisable();
        }

        private void OnEnable()
        {
            //_myMouse = Mouse.current;
            SpellUiEnable();
        }

        private void Update()
        {
            _canCast = false;
            var ray = _mainCamera.ViewportPointToRay(_screenCenter);
            if(Physics.Raycast(ray, out _hitInfo, MaxDistance, spellTargetable)){
                _canCast = true;
                radialSpellUi.SetActive(true);
                var pos = _hitInfo.point;
                pos.y += 0.15f;
                radialSpellUi.transform.position = pos;
                radialSpellUi.transform.forward = _hitInfo.normal * -1f;
            }
            if (!_canCast) return;

            /*if (_myMouse.leftButton.isPressed || casting)
            {
                if (_spellBehaviour == null) return;
                _spellBehaviour.PlayParticleEffect(_hitInfo.point, radialSpellUi.transform.forward);
                _spellBehaviour = null;
                SpellUiDisable();
            }*/
        }

        public void LoadSpellBehaviour(SpellBehaviour behaviour, float range)
        {
            enabled = true;
            _spellBehaviour = behaviour;
            range *= 2.5f;
            radialSpellUi.transform.localScale = new Vector3(range, range, 1f);
        }

        public void SpellUiEnable()
        {
            _canCast = false;
            radialSpellUi.SetActive(true);
        }

        public void SpellUiDisable()
        {
            radialSpellUi.SetActive(false);
            enabled = false;
        }
    }
}
