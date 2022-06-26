using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorArises.BehaviorTree
{
    public class RandomSequencer : Node
    {
    	override public NS Tick(){
    		return NS.Failure;
    	}
    }
}
