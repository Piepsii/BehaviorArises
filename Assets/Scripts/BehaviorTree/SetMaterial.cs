using System.Collections.Generic;
using UnityEngine;

namespace BehaviorArises.BehaviorTree
{
    public class SetMaterial : Node
    {
        MeshRenderer meshRenderer;
        Material material;

        public SetMaterial(Dictionary<string, GameObject> blackboard, Material material)
        {
            meshRenderer = blackboard["gameObject"].GetComponent<MeshRenderer>();
            this.material = material;
        }

        public override NodeState Tick(float deltaTime)
        {
            meshRenderer.material = material;
            return NodeState.Success;
        }
    }
}