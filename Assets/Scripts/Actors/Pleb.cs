using System.Collections.Generic;
using UnityEngine;
using BehaviorArises.BehaviorTree;

namespace BehaviorArises.Actors
{
    enum PlebState
    {
        Patrol,
        MeleeCombat
    }

    public class Pleb : MonoBehaviour{

        public float combatRange = 5f;
        public Material patrolMaterial;
        public Material combatMaterial;

        private PlebState state;
        private Node patrolTree, meleeCombatTree;
        private Dictionary<string, GameObject> blackboard;

        public void Build(){
            blackboard = new Dictionary<string, GameObject>();
            blackboard.Add("gameObject", gameObject);
            blackboard.Add("playerGO", null);

            SetMaterial setPatrolMaterial = new SetMaterial(blackboard, patrolMaterial);
            SetMaterial setCombatMaterial = new SetMaterial(blackboard, combatMaterial);
            DebugLog debugLogPatrol = new DebugLog("I am patrolling!");
            DebugLog debugLogCombat = new DebugLog("I am in combat!");
            Sequencer patrolSequencer = new Sequencer(new List<Node> { setPatrolMaterial, debugLogPatrol });
            Sequencer combatSequencer = new Sequencer(new List<Node> { setCombatMaterial, debugLogCombat });
            patrolTree = patrolSequencer;
            meleeCombatTree = combatSequencer;

        }

        public void Sense()
        {
            var playerGO = GameObject.FindWithTag("Player");
            if (blackboard.ContainsKey("playerGO"))
            {
                blackboard["playerGO"] = playerGO;
            }
        }

        public void Decide()
        {
            var distanceVector = blackboard["playerGO"].transform.position - transform.position;
            var distance = distanceVector.magnitude;
            if (distance < combatRange)
            {
                state = PlebState.MeleeCombat;
            }
            else
            {
                state = PlebState.Patrol;
            }
        }

        public void Tick(float deltaTime){
            switch (state)
            {
                case PlebState.Patrol:
                    patrolTree.Tick(deltaTime);
                    break;
                case PlebState.MeleeCombat:
                    meleeCombatTree.Tick(deltaTime);
                    break;
            }
        }
    }
}