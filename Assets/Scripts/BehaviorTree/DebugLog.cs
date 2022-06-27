using System.Collections.Generic;
using UnityEngine;


namespace BehaviorArises.BehaviorTree
{
    public class DebugLog : Node 
    {

        string logMessage;

        public DebugLog(string logMessage){
            this.logMessage = logMessage;
        }

        public override NodeState Tick(float deltaTime){
            Debug.Log(this.logMessage);
            return NodeState.Success;
        }
    }
}