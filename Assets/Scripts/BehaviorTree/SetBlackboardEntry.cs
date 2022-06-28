using System.Collections.Generic;
using UnityEngine;

namespace BehaviorArises.BehaviorTree
{
    public class SetBlackboardEntry : Node
    {
        private string key;
        private GameObject value;
        private Dictionary<string, GameObject> blackboard;

        public SetBlackboardEntry(Dictionary<string, GameObject> blackboard, string key, GameObject value)
        {
            this.blackboard = blackboard;
            this.key = key;
            this.value = value;
        }

        public override NodeState Tick(float deltaTime)
        {
            if (blackboard.ContainsKey(key))
                blackboard[key] = value;
            else
                blackboard.Add(key, value);
            return NodeState.Success;
        }

    }
}