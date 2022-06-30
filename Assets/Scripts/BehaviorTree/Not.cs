using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorArises.BehaviorTree
{
    public class Not : Node
    {
        public Node child;

        public Not(Node child)
        {
            this.child = child;
        }

        public override NodeState Tick(float deltaTime)
        {
            var childState = child.Tick(deltaTime);
            if(childState != NodeState.Failure)
            {
                return NodeState.Failure;
            }
            else
            {
                return NodeState.Success;
            }
        }

    }
}