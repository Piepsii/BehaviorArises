using System.Collections.Generic;
using UnityEngine;


namespace BehaviorArises.BehaviorTree
{
    public class Attack : Node 
    {
        private int stepsSinceLastAttack = 0;
        private int cooldown;
        private ParticleSystem particleSystem;

        public Attack(ParticleSystem particleSystem, int cooldown){
            this.particleSystem = particleSystem;
            this.cooldown = cooldown;
        }

        public override NodeState Tick(float deltaTime){
            stepsSinceLastAttack++;
            if(stepsSinceLastAttack < cooldown){

                return NodeState.Failure;
            }
            else{
                stepsSinceLastAttack = 0;
                particleSystem.Play();
                return NodeState.Success;
            }
        }
    }
}