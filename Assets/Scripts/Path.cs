using System.Collections.Generic;
using UnityEngine;

namespace BehaviorArises
{
    public class Path : MonoBehaviour
    {
        public int activeWaypoint = 0;
        public List<Transform> waypoints;

        public Transform GetActiveWaypoint()
        {
            return waypoints[activeWaypoint];
        }

        public void SetNextWaypointActive()
        {
            activeWaypoint++;
            activeWaypoint %= waypoints.Count;
        }
    }
}