using System.Collections;
using UnityEngine;

namespace BehaviorArises.BehaviorTree
{
    public class WaitOnce : Node
    {
        private float time;
        private float currentTime;

        public WaitOnce(float time)
        {
            this.time = time;
            currentTime = time;
        }

        public override NodeState Tick(float deltaTime)
        {
            if (currentTime <= 0f)
            {
                return NodeState.Success;
            }
            else
            {
                currentTime -= deltaTime;
                return NodeState.Running;
            }
        }

        public void Reset()
        {
            currentTime = time;
        }
    }
}