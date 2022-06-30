using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorArises.BehaviorTree
{
    public class StayNearObject : Node
    {
        private Dictionary<string, GameObject> blackboard;
        private string objName;
        private float range;
        private float leeway = .2f;
        private Vector3 goalPos;
        private Transform actor;
        private NavMeshAgent agent;

        public StayNearObject(Dictionary<string, GameObject> blackboard, string objName, float range)
        {
            this.blackboard = blackboard;
            this.objName = objName;
            this.range = range;
            actor = blackboard["gameObject"].transform;
            agent = blackboard["gameObject"].GetComponent<NavMeshAgent>();
        }

        public override NodeState Tick(float deltaTime)
        {
            if (!blackboard.ContainsKey(objName))
                return NodeState.Failure;

            var objPos = blackboard[objName].transform.position;
            var distanceVec = actor.position - objPos;
            var vecToGoal = distanceVec.normalized * range;
            goalPos = objPos + vecToGoal;
            var distance = (actor.position - goalPos).magnitude;
            agent.SetDestination(goalPos);
            if(Vector3.Distance(agent.pathEndPosition, goalPos) >= 3f)
            {
                return NodeState.Failure;
            }
            else if(distance <= leeway)
            {
                agent.ResetPath();
                return NodeState.Success;
            }
            else
            {
                if(agent.destination != goalPos)
                {
                    agent.SetDestination(goalPos);
                }
                return NodeState.Running;
            }
        }
    }
}