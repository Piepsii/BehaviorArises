using System.Collections.Generic;
using UnityEngine;

namespace BehaviorArises.BehaviorTree
{
    public class SetNextWaypointActive : Node
    {
        private Path path;

        public SetNextWaypointActive(Path path)
        {
            this.path = path;
        }

        public override NodeState Tick(float deltaTime)
        {
            path.SetNextWaypointActive();
            return NodeState.Success;
        }
    }
}