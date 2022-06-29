using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorArises.BehaviorTree
{
    public class GotoPlayer : Node
    {
        private float leeway = 5f;
        private float distance;
        private Vector3 playerPos;
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
            playerPos = blackboard["player"].transform.position;
            var distanceVec = playerPos - actor.position;
            distance = distanceVec.magnitude;
            agent.SetDestination(playerPos);
            if (Vector3.Distance(agent.pathEndPosition, playerPos) >= 1f)
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
                if (agent.destination != playerPos)
                    agent.SetDestination(playerPos);
                return NodeState.Running;
            }
        }
    }
}