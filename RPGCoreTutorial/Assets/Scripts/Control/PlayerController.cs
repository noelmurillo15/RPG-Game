using System;
using UnityEngine;
using RPG.Movement;
using RPG.Attributes;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
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
        
        [SerializeField] private float maxPathLength = 40f;
        [SerializeField] private float maxNavMeshProjectionDistance = 1f;
        [SerializeField] private CursorMapping[] cursorMappings;


        private void Awake()
        {
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
            if (!EventSystem.current.IsPointerOverGameObject()) return false; //  refers to UI gameobject
            SetCursor(CursorType.UI);
            return true;
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RayCastAllSorted();
            foreach (var hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (!raycastable.HandleRayCast(this)) continue;
                    SetCursor(raycastable.GetCursorType());
                    return true;
                }
            }
            return false;
        }

        private static RaycastHit[] RayCastAllSorted()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            //  Sort all raycast hits based on Distance
            float[] distances = new float[hits.Length];
            for (int x = 0; x < hits.Length; x++)
            {
                distances[x] = hits[x].distance;
            }
            Array.Sort(distances, hits);

            return hits;
        }

        private bool InteractWithMovement()
        {
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);

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
            bool hasHit = Physics.Raycast(GetMouseRay(), out var hit);
            if (!hasHit) return false;

            //  Is the raycast hit NavMesh walkable>?
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out var navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            //  Is there a clear path to the NavMesh target location>?
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            if (!hasPath) return false;

            //  Is the path complete>?
            if (path.status != NavMeshPathStatus.PathComplete) return false;

            //  Is path too long>? TODO : if the land is flat and there are no corners in the navmesh, this will cause flat land to be unwalkable
            // if(GetPathLength(path) > maxPathLength) return false;

            target = navMeshHit.position;  //  out position
            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;

            for (int x = 0; x < path.corners.Length - 1; x++)
            { total += Vector3.Distance(path.corners[x], path.corners[x + 1]); }

            return total;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
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
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}