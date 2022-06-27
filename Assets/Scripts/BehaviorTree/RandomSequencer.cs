using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorArises.BehaviorTree
{
    public class RandomSequencer : Node
    {
        public List<Node> children;
        
    	override public NodeState Tick(float deltaTime){
    		return NodeState.Failure;
    	}
    }
}
