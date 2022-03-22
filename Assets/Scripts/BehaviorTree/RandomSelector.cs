using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorArises.BehaviorTree
{
    public class RandomSelector : Node
    {
        public RandomSelector(List<Node> children)
        {
            this.children = children;
        }

        public override NS Tick()
        {
            List<int> indeces = new List<int>(children.Count);
                
            foreach (Node n in children)
            {
                NS ret = n.Tick();
                if (ret == NS.Running
                    || ret == NS.Success)
                    return ret;
            }
            return NS.Failure;
        }
    }
}
