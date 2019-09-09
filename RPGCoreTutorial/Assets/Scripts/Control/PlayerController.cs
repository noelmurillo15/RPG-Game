using UnityEngine;
using RPG.Movement;
using RPG.Resources;
using UnityEngine.EventSystems;
using System;
using UnityEngine.AI;

namespace RPG.Control
{
    [RequireComponent(typeof(CharacterMove))]
    public class PlayerController : MonoBehaviour
    {
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        //  Cached Variables
        Health myHealth;

        //  
        [SerializeField] float maxPathLength = 40f;
        [SerializeField] float maxNavMeshProjectionDistance = 1f;
        [SerializeField] CursorMapping[] cursorMappings;


        void Awake()
        {
            myHealth = GetComponent<Health>();
        }

        void Update()
        {
            if (InteractWithUI()) return;

            if (myHealth.IsDead())
            { SetCursor(CursorType.NONE); return; }

            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;

            SetCursor(CursorType.NONE);
        }

        bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {   //  refers to UI gameobject
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        bool InteractWithComponent()
        {
            RaycastHit[] hits = RayCastAllSorted();
            foreach (var hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRayCast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        RaycastHit[] RayCastAllSorted()
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

        bool InteractWithMovement()
        {
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<CharacterMove>().StartMoveAction(target, 1f);
                }
                SetCursor(CursorType.MOVEMENT);
                return true;
            }
            return false;
        }

        bool RaycastNavMesh(out Vector3 _target)
        {
            _target = new Vector3();

            //  Did the raycast hit anything>?
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;

            //  Is the raycast hit NavMesh walkable>?
            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            //  Is there a clear path to the NavMesh target location>?
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, _target, NavMesh.AllAreas, path);
            if (!hasPath) return false;

            //  Is the path complete>?
            if (path.status != NavMeshPathStatus.PathComplete) return false;

            //  Is path too long>? TODO : if the land is flat and there are no corners in the navmesh, this will cause flat land to be unwalkable
            // if(GetPathLength(path) > maxPathLength) return false;

            _target = navMeshHit.position;  //  out position
            return true;
        }

        float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;

            for (int x = 0; x < path.corners.Length - 1; x++)
            { total += Vector3.Distance(path.corners[x], path.corners[x + 1]); }

            return total;
        }

        void SetCursor(CursorType _type)
        {
            CursorMapping mapping = GetCursorMapping(_type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        CursorMapping GetCursorMapping(CursorType _type)
        {
            foreach (var mapping in cursorMappings)
            {
                if (mapping.type == _type)
                    return mapping;
            }

            return cursorMappings[0];
        }

        static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}