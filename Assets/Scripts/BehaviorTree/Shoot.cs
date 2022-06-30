using System.Collections.Generic;
using UnityEngine;


namespace BehaviorArises.BehaviorTree
{
    public class Shoot : Node
    {
        private Dictionary<string, GameObject> blackboard;
        private GameObject projectile;

        public Shoot(Dictionary<string, GameObject> blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Tick(float deltaTime)
        {
            if (!blackboard.ContainsKey("projectile"))
                return NodeState.Failure;

            projectile = blackboard["projectile"];
            var actor = blackboard["gameObject"];
            GameObject.Instantiate(projectile, actor.transform);
            return NodeState.Success;
        }
    }
}