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

        public Material patrolMaterial;
        public Material combatMaterial;
        public float gotoPlayerLeeway = 2f;
        public float combatRange = 3f;

        private Path path;
        private Camera cam;
        private PlebState state;
        private NavMeshAgent agent;
        private ParticleSystem pSystem;
        private Node patrolTree, meleeCombatTree;
        private int stepsSinceLastSeenPlayer = 300;
        private int cooldownInSteps = 60;
        private Dictionary<string, int> blackboardInt;
        private Dictionary<string, GameObject> blackboard;

        public void Build(){
            blackboard = new Dictionary<string, GameObject>();
            blackboard.Add("gameObject", gameObject);
            blackboard.Add("player", null);
            blackboardInt = new Dictionary<string, int>();
            blackboardInt.Add("stepsSinceLastSeenPlayer", 0);
            agent = GetComponent<NavMeshAgent>();
            path = GetComponent<Path>();
            cam = GetComponent<Camera>();
            pSystem = GetComponent<ParticleSystem>();

            // Patrol Behavior Tree

            SetMaterial setPatrolMaterial = new SetMaterial(blackboard, patrolMaterial);
            GotoNextWaypoint setDestination = new GotoNextWaypoint(blackboard);
            IsNearWaypoint isNearWaypoint = new IsNearWaypoint(blackboard, 2f);
            //SetBlackboardEntry setNextWaypoint = new SetBlackboardEntry(blackboard, "waypoint", waypoint1.gameObject);
            SetNextWaypointActive setNextWaypointActive = new SetNextWaypointActive(path);
            Sequencer materialLogAndGoToWaypoint = new Sequencer(new List<Node> { setPatrolMaterial, setDestination });
            Sequencer setWaypointAtArrival = new Sequencer(new List<Node> { isNearWaypoint, setNextWaypointActive });
            Selector patrolRoot = new Selector(new List<Node> { setWaypointAtArrival, materialLogAndGoToWaypoint });
            patrolTree = patrolRoot;

            // !Patrol Behavior Tree

            // Combat Behavior Tree

            IsNearObject isNearPlayer = new IsNearObject(blackboard, "player", combatRange);
            TurnTowardsObject turnTowardsPlayer = new TurnTowardsObject(blackboard, "player", 0.8f, 30f);
            Attack attack = new Attack(pSystem, cooldownInSteps);
            Sequencer attackSequence = new Sequencer(new List<Node> { isNearPlayer, turnTowardsPlayer, attack });

            GotoPlayer gotoPlayer = new GotoPlayer(blackboard, gotoPlayerLeeway);
            Selector goToPlayerAndAttack = new Selector(new List<Node> { attackSequence, gotoPlayer});

            SetMaterial setCombatMaterial = new SetMaterial(blackboard, combatMaterial);
            Sequencer combatRoot = new Sequencer(new List<Node> { setCombatMaterial, goToPlayerAndAttack });
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