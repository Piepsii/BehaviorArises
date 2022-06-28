using System.Collections.Generic;
using UnityEngine;

namespace BehaviorArises.BehaviorTree
{
    public class SetNextWaypointActive : Node
    {
        private Dictionary<string, GameObject> blackboard;

        public SetNextWaypointActive(Dictionary<string, GameObject> blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Tick(float deltaTime)
        {
            blackboard["gameObject"].GetComponent<Path>().SetNextWaypointActive();
            return NodeState.Success;
        }
    }
}