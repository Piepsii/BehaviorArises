using System.Collections.Generic;
using UnityEngine;

namespace BehaviorArises.BehaviorTree
{
    public class IsNearObject : Node
    {
        private Dictionary<string, GameObject> blackboard;
        private string objectName;
        private float maxDistance;

        public IsNearObject(Dictionary<string, GameObject> blackboard, string objectName, float maxDistance = 2f)
        {
            this.blackboard = blackboard;
            this.objectName = objectName;
            this.maxDistance = maxDistance;
        }

        public override NodeState Tick(float deltaTime)
        {
            if (!blackboard.ContainsKey(objectName))
            {
                Debug.Log("IsNearObject : Node. Variable 'objectName' not found in blackboard.");
                return NodeState.Failure;
            }
            var objectPosition = blackboard[objectName].transform.position;
            var actorPosition = blackboard["gameObject"].transform.position;
            var distance = Vector3.Distance(objectPosition, actorPosition);
            if(distance < maxDistance)
            {
                return NodeState.Success;
            }
            return NodeState.Failure;
        }
    }
}