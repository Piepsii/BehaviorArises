using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorArises.BehaviorTree
{ 
    public enum NS
    {
        Failure,
        Success,
        Running
    }

    public abstract class Node
    {
        public abstract NS Tick();
        public List<Node> children;
    }

}
