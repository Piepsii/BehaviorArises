using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorArises.BehaviorTree
{
    public class RandomSelector : Node
    {
        public List<Node> children;

        public RandomSelector(List<Node> children)
        {
            this.children = children;
        }

        public override NodeState Tick(float deltaTime)
        {   
            List<int> indeces = new List<int>(children.Count);
                
            foreach (Node n in children)
            {
                NodeState result = n.Tick(deltaTime);
                if (result == NodeState.Running || result == NodeState.Success)
                    return result;
            }
            return NodeState.Failure;
        }
    }
}
