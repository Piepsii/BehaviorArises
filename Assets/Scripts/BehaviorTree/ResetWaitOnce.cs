using System.Collections;
using UnityEngine;

namespace BehaviorArises.BehaviorTree
{
    public class ResetWaitOnce : Node
    {
        private WaitOnce waitNode;

        public ResetWaitOnce(WaitOnce waitNode)
        {
            this.waitNode = waitNode;
        }

        public override NodeState Tick(float deltaTime)
        {
            waitNode.Reset();
            return NodeState.Success;
        }
    }
}