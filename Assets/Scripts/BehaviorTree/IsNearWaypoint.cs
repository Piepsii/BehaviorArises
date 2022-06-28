using System.Collections.Generic;
using UnityEngine;

namespace BehaviorArises.BehaviorTree
{
    public class IsNearWaypoint : Node
    {
        private float maxDistance;
        private Vector3 actorPosition;
        private Vector3 waypoint;
        private Dictionary<string, GameObject> blackboard;

        public IsNearWaypoint(Dictionary<string, GameObject> blackboard, float maxDistance) {
            this.maxDistance = maxDistance;
            this.blackboard = blackboard;
        }

        public override NodeState Tick(float deltaTime)
        {
            actorPosition = blackboard["gameObject"].transform.position;
            waypoint = blackboard["gameObject"].GetComponent<Path>().GetActiveWaypoint().position;
            var distance = Vector3.Distance(actorPosition, waypoint);
            if(distance < maxDistance)
            {
                return NodeState.Success;
            }
            else
            {
                return NodeState.Failure;
            }
        }
    }
}