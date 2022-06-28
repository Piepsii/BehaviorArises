using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorArises.BehaviorTree
{
    public class GotoNextWaypoint : Node
    {
        private float leeway = 2f;
        private float distance;
        private Vector3 waypoint;
        private Transform actor;
        private NavMeshAgent agent;
        private Dictionary<string, GameObject> blackboard;

        public GotoNextWaypoint(Dictionary<string, GameObject> blackboard)
        {
            this.blackboard = blackboard;
            var gameObject = blackboard["gameObject"];
            actor = gameObject.transform;
            agent = gameObject.GetComponent<NavMeshAgent>();
        }
        
        public override NodeState Tick(float deltaTime)
        {
            waypoint = blackboard["gameObject"].GetComponent<Path>().GetActiveWaypoint().position;
            var distanceVec = waypoint - actor.position;
            distance = distanceVec.magnitude;
            agent.SetDestination(waypoint);
            if (Vector3.Distance(agent.pathEndPosition, waypoint) >= 1f)
            {
                return NodeState.Failure;
            }
            else if(distance <= leeway)
            {
                return NodeState.Success;
            }
            else
            {
                if(agent.destination != waypoint)
                    agent.SetDestination(waypoint);
                return NodeState.Running;
            }
        }
    }
}