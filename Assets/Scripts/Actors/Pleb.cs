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

        private int stepsSinceLastSeenPlayer = 300;
        private Path path;
        private Camera cam;
        private PlebState state;
        private NavMeshAgent agent;
        private Node patrolTree, meleeCombatTree;
        private Dictionary<string, GameObject> blackboard;
        private Dictionary<string, int> blackboardInt;

        public void Build(){
            blackboard = new Dictionary<string, GameObject>();
            blackboard.Add("gameObject", gameObject);
            blackboard.Add("player", null);
            blackboardInt = new Dictionary<string, int>();
            blackboardInt.Add("stepsSinceLastSeenPlayer", 0);
            agent = GetComponent<NavMeshAgent>();
            path = GetComponent<Path>();
            cam = GetComponent<Camera>();

            // Patrol Behavior Tree

            SetMaterial setPatrolMaterial = new SetMaterial(blackboard, patrolMaterial);
            GotoNextWaypoint setDestination = new GotoNextWaypoint(blackboard);
            IsNearWaypoint isNearWaypoint = new IsNearWaypoint(blackboard, 2f);
            //SetBlackboardEntry setNextWaypoint = new SetBlackboardEntry(blackboard, "waypoint", waypoint1.gameObject);
            SetNextWaypointActive setNextWaypointActive = new SetNextWaypointActive(blackboard);
            DebugLog debugLogPatrol = new DebugLog("I am patrolling!");
            Sequencer materialLogAndGoToWaypoint = new Sequencer(new List<Node> { setPatrolMaterial, debugLogPatrol, setDestination });
            Sequencer setWaypointAtArrival = new Sequencer(new List<Node> { isNearWaypoint, setNextWaypointActive });
            Selector patrolRoot = new Selector(new List<Node> { setWaypointAtArrival, materialLogAndGoToWaypoint });
            patrolTree = patrolRoot;

            // !Patrol Behavior Tree

            // Combat Behavior Tree

            SetMaterial setCombatMaterial = new SetMaterial(blackboard, combatMaterial);
            DebugLog debugLogCombat = new DebugLog("I am in combat!");
            GotoPlayer gotoPlayer = new GotoPlayer(blackboard);
            Sequencer combatRoot = new Sequencer(new List<Node> { setCombatMaterial, debugLogCombat, gotoPlayer });
            meleeCombatTree = combatRoot;

            // !Combat Behavior Tree
        }

        public void Sense()
        {
            var playerGO = GameObject.FindWithTag("Player");
            if (blackboard.ContainsKey("player"))
            {
                blackboard["player"] = playerGO;
            }

            var playerCollider = playerGO.GetComponent<Collider>();
            Plane[] planes;
            planes = GeometryUtility.CalculateFrustumPlanes(cam);
            if (GeometryUtility.TestPlanesAABB(planes, playerCollider.bounds) )
            // && !Physics.Linecast(transform.position, playerGO.transform.position, ~LayerMask.NameToLayer("Enemy"))
            {
                stepsSinceLastSeenPlayer = 0;
            }
            else
            {
                stepsSinceLastSeenPlayer++;
            }
        }

        public void Decide()
        {
            if (stepsSinceLastSeenPlayer < 100)
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