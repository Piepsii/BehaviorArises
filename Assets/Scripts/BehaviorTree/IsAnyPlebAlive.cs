using System.Collections;
using UnityEngine;

namespace BehaviorArises.BehaviorTree
{
    public class IsAnyPlebAlive : Node
    {
        private GameObject[] plebs;

        public IsAnyPlebAlive(GameObject[] plebs)
        {
            this.plebs = plebs;
        }

        public override NodeState Tick(float deltaTime)
        {
            for(int i = 0; i < plebs.Length; i++)
            {
                if (plebs[i] != null)
                    return NodeState.Success;
            }
            return NodeState.Failure;
        }
    }
}