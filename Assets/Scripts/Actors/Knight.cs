using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorArises.BehaviorTree;

namespace BehaviorArises.Actors
{
    enum KnightState
    {
        Patrol,
        MeleeCombat
    }

    public class Knight : MonoBehaviour{

        public Material patrolMaterial, combatMaterial;

        private float stayNearPlayerRange = 7f;
        private float startleTime = 1f;
        private float gotoPlayerLeeway = 3.1f;
        private float combatRange = 3f;

        private int stepsSinceLastSeenPlayer = 1000;
        private int cooldownInSteps = 90;
        private Node patrolTree, meleeCombatTree;
        private KnightState state;
        private Path path;
        private Camera cam;
        private NavMeshAgent agent;
        private ParticleSystem pSystem;
        private Dictionary<string, GameObject> blackboard;
        private Dictionary<string, int> blackboardInt;

        public void Build(GameObject[] plebs)
        {
            blackboard = new Dictionary<string, GameObject>();
            blackboard.Add("gameObject", gameObject);
            blackboard.Add("player", null);
            blackboardInt = new Dictionary<string, int>();
            blackboardInt.Add("stepsSinceLastSeenPlayer", 0);
            agent = GetComponent<NavMeshAgent>();
            path = GetComponent<Path>();
            cam = GetComponent<Camera>();
            pSystem = GetComponent<ParticleSystem>();

            // General Nodes
            WaitOnce startle = new WaitOnce(startleTime);
            ResetWaitOnce resetStartle = new ResetWaitOnce(startle);

            // Patrol Behavior Tree

            SetMaterial setPatrolMaterial = new SetMaterial(blackboard, patrolMaterial);
            GotoNextWaypoint setDestination = new GotoNextWaypoint(blackboard);
            IsNearWaypoint isNearWaypoint = new IsNearWaypoint(blackboard, 2f);
            SetNextWaypointActive setNextWaypointActive = new SetNextWaypointActive(path);
            Sequencer setMaterialGotoWaypoint = new Sequencer(new List<Node> { resetStartle, setPatrolMaterial, setDestination });
            Sequencer setWaypointAtArrival = new Sequencer(new List<Node> { isNearWaypoint, setNextWaypointActive });
            Selector patrolRoot = new Selector(new List<Node> { setWaypointAtArrival, setMaterialGotoWaypoint });
            patrolTree = patrolRoot;

            // !Patrol Behavior Tree

            // Combat Behavior Tree

            TurnTowardsObject turnTowardsPlayer = new TurnTowardsObject(blackboard, "player", 0.5f, 40f);

            IsAnyPlebAlive isAnyPlebAlive = new IsAnyPlebAlive(plebs);
            StayNearObject stayNearPlayer = new StayNearObject(blackboard, "player", stayNearPlayerRange);
            Sequencer stayAwaySequence = new Sequencer(new List<Node> { isAnyPlebAlive, stayNearPlayer, turnTowardsPlayer });

            IsNearObject isNearPlayer = new IsNearObject(blackboard, "player", combatRange);
            Attack attack = new Attack(pSystem, cooldownInSteps);
            Sequencer attackSequence = new Sequencer(new List<Node> { isNearPlayer, turnTowardsPlayer, attack });

            GotoPlayer gotoPlayer = new GotoPlayer(blackboard, gotoPlayerLeeway);
            Selector goToPlayerAndAttack = new Selector(new List<Node> { attackSequence, gotoPlayer });

            SetMaterial setCombatMaterial = new SetMaterial(blackboard, combatMaterial);
            Selector stayAwayOrAttackSelector = new Selector(new List<Node> { stayAwaySequence, goToPlayerAndAttack });

            Sequencer combatRoot = new Sequencer(new List<Node> { setCombatMaterial, startle, stayAwayOrAttackSelector});
            meleeCombatTree = combatRoot;

            // !Combat Behavior Tree
        }

        public void Sense()
        {
            var player = GameObject.FindWithTag("Player");
            if (blackboard.ContainsKey("player"))
                blackboard["player"] = player;
            var playerCollider = player.GetComponent<Collider>();
            Plane[] planes;
            planes = GeometryUtility.CalculateFrustumPlanes(cam);
            if (GeometryUtility.TestPlanesAABB(planes, playerCollider.bounds))
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
            if(stepsSinceLastSeenPlayer < 450)
            {
                state = KnightState.MeleeCombat;
            }
            else
            {
                state = KnightState.Patrol;
            }
        }

        public void Tick(float deltaTime)
        {
            switch (state)
            {
                case KnightState.Patrol:
                    patrolTree.Tick(deltaTime);
                    break;
                case KnightState.MeleeCombat:
                    meleeCombatTree.Tick(deltaTime);
                    break;
            }
        }
    }
}