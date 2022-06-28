using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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

        private Path path;
        private PlebState state;
        private NavMeshAgent agent;
        private Node patrolTree, meleeCombatTree;
        private Dictionary<string, GameObject> blackboard;

        public void Build(){
            blackboard = new Dictionary<string, GameObject>();
            blackboard.Add("gameObject", gameObject);
            blackboard.Add("playerGO", null);
            agent = GetComponent<NavMeshAgent>();
            path = GetComponent<Path>();

            SetMaterial setPatrolMaterial = new SetMaterial(blackboard, patrolMaterial);
            SetMaterial setCombatMaterial = new SetMaterial(blackboard, combatMaterial);
            SetDestination setDestination = new SetDestination(blackboard);
            IsNearWaypoint isNearWaypoint = new IsNearWaypoint(blackboard, 2f);
            //SetBlackboardEntry setNextWaypoint = new SetBlackboardEntry(blackboard, "waypoint", waypoint1.gameObject);
            SetNextWaypointActive setNextWaypointActive = new SetNextWaypointActive(blackboard);
            DebugLog debugLogPatrol = new DebugLog("I am patrolling!");
            DebugLog debugLogCombat = new DebugLog("I am in combat!");
            Sequencer materialLogAndGoToWaypoint = new Sequencer(new List<Node> { setPatrolMaterial, debugLogPatrol, setDestination });
            Sequencer combatRoot = new Sequencer(new List<Node> { setCombatMaterial, debugLogCombat });
            Sequencer setWaypointAtArrival = new Sequencer(new List<Node> { isNearWaypoint, setNextWaypointActive });
            Selector patrolRoot = new Selector(new List<Node> { setWaypointAtArrival, materialLogAndGoToWaypoint });
            patrolTree = patrolRoot;
            meleeCombatTree = combatRoot;

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