using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorArises.BehaviorTree;

namespace BehaviorArises.Actors
{
    enum CrossbowmanState
    {
        Patrol,
        MeleeCombat,
        RangedCombat
    }

    public class Crossbowman : MonoBehaviour
    {

        [SerializeField] private Material patrolMaterial, meleeMaterial, rangedMaterial;
        [SerializeField] private float stayNearPlayerRange = 2f;

        private int stepsSinceLastSeenPlayer = 1000;
        private int cooldownInSteps = 90;
        private Node patrolTree, meleeCombatTree;
        private CrossbowmanState state;
        private Path path;
        private Camera cam;
        private NavMeshAgent agent;
        private ParticleSystem pSystem;
        private Dictionary<string, GameObject> blackboard;
        private Dictionary<string, int> blackboardInt;

        public void Build()
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

            // Patrol Behavior Tree

            SetMaterial setPatrolMaterial = new SetMaterial(blackboard, patrolMaterial);
            GotoNextWaypoint setDestination = new GotoNextWaypoint(blackboard);
            IsNearWaypoint isNearWaypoint = new IsNearWaypoint(blackboard, 2f);
            SetNextWaypointActive setNextWaypointActive = new SetNextWaypointActive(path);
            Sequencer setMaterialGotoWaypoint = new Sequencer(new List<Node> { setPatrolMaterial, setDestination });
            Sequencer setWaypointAtArrival = new Sequencer(new List<Node> { isNearWaypoint, setNextWaypointActive });
            Selector patrolRoot = new Selector(new List<Node> { setWaypointAtArrival, setMaterialGotoWaypoint });
            patrolTree = patrolRoot;

            // !Patrol Behavior Tree

            // Combat Behavior Tree
            TurnTowardsObject turnTowardsPlayer = new TurnTowardsObject(blackboard, "player", 0.5f, 30f);

            StayNearObject stayNearPlayer = new StayNearObject(blackboard, "player", stayNearPlayerRange);
            Sequencer stayAwaySequence = new Sequencer(new List<Node> { stayNearPlayer, turnTowardsPlayer });

            GotoPlayer gotoPlayer = new GotoPlayer(blackboard);
            Attack attack = new Attack(pSystem, cooldownInSteps);
            Sequencer attackPlayerSequence = new Sequencer(new List<Node> { gotoPlayer, turnTowardsPlayer, attack });

            SetMaterial setCombatMaterial = new SetMaterial(blackboard, meleeMaterial);
            Selector stayAwayOrAttackSelector = new Selector(new List<Node> { stayAwaySequence, attackPlayerSequence });

            Sequencer combatRoot = new Sequencer(new List<Node> { setCombatMaterial, stayAwayOrAttackSelector });
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
            if (stepsSinceLastSeenPlayer < 450)
            {
                state = CrossbowmanState.MeleeCombat;
            }
            else
            {
                state = CrossbowmanState.Patrol;
            }
        }

        public void Tick(float deltaTime)
        {
            switch (state)
            {
                case CrossbowmanState.Patrol:
                    patrolTree.Tick(deltaTime);
                    break;
                case CrossbowmanState.MeleeCombat:
                    meleeCombatTree.Tick(deltaTime);
                    break;
                case CrossbowmanState.RangedCombat:
                    meleeCombatTree.Tick(deltaTime);
                    break;
            }
        }
    }
}