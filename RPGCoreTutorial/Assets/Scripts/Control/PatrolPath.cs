/*
 * PatrolPath - 
 * Created by : Allan N. Murillo
 * Last Edited : 2/25/2020
 */

using UnityEngine;

namespace ANM.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private const float WaypointGizmosRadius = 0.3f;

        private void OnDrawGizmos()
        {
            for (var x = 0; x < transform.childCount; x++)
            {
                var y = GetNextIndex(x);
                Gizmos.DrawSphere(GetWaypoint(x), WaypointGizmosRadius);
                Gizmos.DrawLine(GetWaypoint(x), GetWaypoint(y));
            }
        }

        public int GetNextIndex(int x)
        {
            if ((x + 1) >= transform.childCount)
                return 0;

            return x + 1;
        }

        public Vector3 GetWaypoint(int x)
        {
            return transform.GetChild(x).position;
        }
    }
}