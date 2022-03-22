using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorArises.BehaviorTree
{
    public class Sequencer : Node
    {
        public Sequencer(List<Node> children) 
        {
            this.children = children;
        }

        public override NS Tick()
        {
            foreach(Node n in children)
            {
                NS ret = n.Tick();
                if (ret == NS.Failure 
                    || ret == NS.Running)
                    return ret;
            }
            return NS.Success;
        }
    }
}