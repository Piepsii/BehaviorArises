using System.Collections;
using UnityEngine;

namespace BehaviorArises.BehaviorTree
{
    public class Wait : Node
    {
        private float time;
        private float currentTime;

        public Wait(float time)
        {
            this.time = time;
            currentTime = time;
        }

        public override NodeState Tick(float deltaTime)
        {
            if(currentTime <= 0f)
            {
                currentTime = time;
                return NodeState.Success;
            }
            else
            {
                currentTime -= deltaTime;
                return NodeState.Running;
            }
        }
    }
}