using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorArises.BehaviorTree
{
    public class GotoPlayer : Node
    {
        private float leeway = 5f;
        private float distance;
        private Vector3 playerPosition;
        private Transform actor;
        private NavMeshAgent agent;
        private Dictionary<string, GameObject> blackboard;

        public GotoPlayer(Dictionary<string, GameObject> blackboard)
        {
            this.blackboard = blackboard;
            var gameObject = blackboard["gameObject"];
            actor = gameObject.transform;
            agent = gameObject.GetComponent<NavMeshAgent>();
        }

        public override NodeState Tick(float deltaTime)
        {
            playerPosition = blackboard["player"].transform.position;
            var distanceVec = playerPosition - actor.position;
            distance = distanceVec.magnitude;
            agent.SetDestination(playerPosition);
            if (Vector3.Distance(agent.pathEndPosition, playerPosition) >= 1f)
            {
                return NodeState.Failure;
            }
            else if (distance <= leeway)
            {
                agent.ResetPath();
                return NodeState.Success;
            }
            else
            {
                if (agent.destination != playerPosition)
                    agent.SetDestination(playerPosition);
                return NodeState.Running;
            }
        }
    }
}