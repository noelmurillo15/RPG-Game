/*
 * PlayerController - 
 * Created by : Allan N. Murillo
 * Last Edited : 2/25/2020
 */

using System;
using UnityEngine;
using ANM.Movement;
using ANM.Attributes;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace ANM.Control
{
    [RequireComponent(typeof(CharacterMove))]
    public class PlayerController : MonoBehaviour
    {
        [Serializable]
        private struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        //  Cached Variables
        private Health _myHealth;
        
        [SerializeField] private float maxPathLength = 200f;
        [SerializeField] private float maxNavMeshProjectionDistance = 1f;
        [SerializeField] private CursorMapping[] cursorMappings;
        private static Camera _mainCam;


        private void Awake()
        {
            _mainCam = Camera.main;
            _myHealth = GetComponent<Health>();
        }

        private void Update()
        {
            if (InteractWithUi()) return;

            if (_myHealth.IsDead())
            { SetCursor(CursorType.NONE); return; }

            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;

            SetCursor(CursorType.NONE);
        }

        private bool InteractWithUi()
        {
            if (EventSystem.current == null) return false;
            if (!EventSystem.current.IsPointerOverGameObject()) return false; //  refers to UI game object
            SetCursor(CursorType.UI);
            return true;
        }

        private bool InteractWithComponent()
        {
            IEnumerable<RaycastHit> hits = RayCastAllSorted();
            foreach (var hit in hits)
            {
                var interfaces = hit.transform.GetComponents<IRaycastable>();
                foreach (var raycastable in interfaces)
                {
                    if (!raycastable.HandleRayCast(this)) continue;
                    SetCursor(raycastable.GetCursorType());
                    return true;
                }
            }
            return false;
        }

        private static IEnumerable<RaycastHit> RayCastAllSorted()
        {
            var hits = Physics.RaycastAll(GetMouseRay());

            //  Sort all raycast hits based on Distance
            var distances = new float[hits.Length];
            for (var x = 0; x < hits.Length; x++)
            {
                distances[x] = hits[x].distance;
            }
            Array.Sort(distances, hits);

            return hits;
        }

        private bool InteractWithMovement()
        {
            var hasHit = RaycastNavMesh(out var target);
            if (!hasHit) return false;
            if (Input.GetMouseButton(0))
            {
                GetComponent<CharacterMove>().StartMoveAction(target, 1f);
            }
            SetCursor(CursorType.MOVEMENT);
            return true;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();

            //  Did the raycast hit anything>?
            var hasHit = Physics.Raycast(GetMouseRay(), out var hit);
            if (!hasHit) return false;
            
            //  Is the raycast hit NavMesh walkable>?
            var hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out var navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;
            
            //  Is there a clear path to the NavMesh target location>?
            var path = new NavMeshPath();
            var hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            
            target = navMeshHit.position;
            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;

            for (var x = 0; x < path.corners.Length - 1; x++)
            { total += Vector3.Distance(path.corners[x], path.corners[x + 1]); }

            return total;
        }

        private void SetCursor(CursorType type)
        {
            var mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (var mapping in cursorMappings)
            {
                if (mapping.type == type)
                    return mapping;
            }

            return cursorMappings[0];
        }

        private static Ray GetMouseRay()
        {
            return _mainCam.ScreenPointToRay(Input.mousePosition);
        }
    }
}