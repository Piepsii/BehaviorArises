using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorArises.BehaviorTree
{ 
    public enum NodeState
    {
        Failure,
        Success,
        Running
    }

    public abstract class Node
    {
        public abstract NodeState Tick(float deltaTime);
    }

}
