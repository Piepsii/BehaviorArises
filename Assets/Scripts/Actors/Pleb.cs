using UnityEngine;
using BehaviorArises.BehaviorTree;

namespace BehaviorArises.Actors
{
    public class Pleb : MonoBehaviour{
        private Node tree;

        public void Build(){
            tree = new DebugLog("I am alive!"); 
        }

        public NodeState Tick(float deltaTime){
            return tree.Tick(deltaTime);
        }
    }
}