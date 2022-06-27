using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorArises.BehaviorTree
{
    public class Sequencer : Node
    {
        public List<Node> children;

        public Sequencer(List<Node> children) 
        {
            this.children = children;
        }

        public override NodeState Tick(float deltaTime)
        {
            foreach(Node n in children)
            {
                NodeState result = n.Tick(deltaTime);
                if (result == NodeState.Running || result == NodeState.Failure)
                    return result;
            }
            return NodeState.Success;
        }
    }
}