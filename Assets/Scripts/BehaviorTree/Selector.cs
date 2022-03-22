using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorArises.BehaviorTree
{
    public class Selector : Node
    {
        public Selector(List<Node> children)
        {
            this.children = children;
        }

        public override NS Tick()
        {
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