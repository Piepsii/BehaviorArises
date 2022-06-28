using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorArises.BehaviorTree
{
    public class TurnTowardsObject : Node
    {
        private float leeway;
        private Transform actor;
        private string objectName;
        private float rotationSpeed;
        private Dictionary<string, GameObject> blackboard;

        public TurnTowardsObject(Dictionary<string, GameObject> blackboard, string objectName, float rotationSpeed = 1f, float leeway = 1f)
        {
            this.blackboard = blackboard;
            this.objectName = objectName;
            this.rotationSpeed = rotationSpeed;
            this.leeway = leeway;
            actor = blackboard["gameObject"].transform;
        }

        public override NodeState Tick(float deltaTime)
        {
            var obj = blackboard[objectName];
            var relativePos = obj.transform.position - actor.position;
            Quaternion lookAtObjectRotation = Quaternion.LookRotation(relativePos, Vector3.up);
            if (Quaternion.Angle(actor.rotation, lookAtObjectRotation) <= leeway)
            {
                return NodeState.Success;
            }
            else
            {
                var step = rotationSpeed * deltaTime * 100f;
                actor.rotation = Quaternion.RotateTowards(actor.rotation, lookAtObjectRotation, step);
                return NodeState.Running;
            }
        }
    }
}