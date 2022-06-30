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

        [SerializeField] private ParticleSystem pSystemMelee;
        [SerializeField] private Material patrolMaterial, meleeMaterial, rangedMaterial;
        [SerializeField] private float rangedCombatRange = 10f;
        [SerializeField] private float meleeCombatRange = 5f;
        [SerializeField] private int meleeCooldown = 180, rangedCooldown = 90;
        [SerializeField] private GameObject projectile;
        [SerializeField] private float reloadTime = 2f, startleTime = 1f;

        private int stepsSinceLastSeenPlayer = 1000;
        private Node patrolTree, meleeCombatTree, rangedCombatTree;
        private CrossbowmanState state;
        private Path path;
        private Camera cam;
        private NavMeshAgent agent;
        private Dictionary<string, GameObject> blackboard;
        private Dictionary<string, int> blackboardInt;

        public void Build()
        {
            blackboard = new Dictionary<string, GameObject>();
            blackboard.Add("gameObject", gameObject);
            blackboard.Add("player", null);
            blackboard.Add("projectile", projectile);

            blackboardInt = new Dictionary<string, int>();
            blackboardInt.Add("stepsSinceLastSeenPlayer", 0);
            agent = GetComponent<NavMeshAgent>();
            path = GetComponent<Path>();
            cam = GetComponent<Camera>();


            // General
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

            // Melee Combat Behavior Tree


            TurnTowardsObject turnTowardsPlayerMelee = new TurnTowardsObject(blackboard, "player", 1.0f, 25f);

            GotoPlayer gotoPlayer = new GotoPlayer(blackboard);
            Attack attack = new Attack(pSystemMelee, meleeCooldown);
            Sequencer attackPlayerSequence = new Sequencer(new List<Node> { gotoPlayer, turnTowardsPlayerMelee, attack });

            SetMaterial setCombatMaterial = new SetMaterial(blackboard, meleeMaterial);

            Sequencer combatRoot = new Sequencer(new List<Node> { setCombatMaterial, startle, attackPlayerSequence });
            meleeCombatTree = combatRoot;

            // !Melee Combat Behavior Tree

            // Ranged Combat Behavior Tree

            TurnTowardsObject turnTowardsPlayer = new TurnTowardsObject(blackboard, "player", 1.5f, 20f);
            Wait reload = new Wait(reloadTime);
            Shoot shootBolt = new Shoot(blackboard);
            Sequencer rangedAttackSequence = new Sequencer(new List<Node> { turnTowardsPlayer, reload, shootBolt });

            IsNearObject isNearPlayer = new IsNearObject(blackboard, "player", rangedCombatRange);
            StayNearObject moveAway = new StayNearObject(blackboard, "player", rangedCombatRange + 5f);
            Sequencer moveAwaySequence = new Sequencer(new List<Node> { isNearPlayer, moveAway });

            Selector distanceSelector = new Selector(new List<Node> { moveAwaySequence, rangedAttackSequence });

            SetMaterial setRangedCombatMaterial = new SetMaterial(blackboard, rangedMaterial);
            Sequencer rangedCombatRoot = new Sequencer(new List<Node> { setRangedCombatMaterial, startle, distanceSelector });
            rangedCombatTree = rangedCombatRoot;

            // !Ranged Combat Behavior Tree
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
            state = CrossbowmanState.Patrol;

            var player = blackboard["player"].transform;
            var distanceToPlayer = (player.position - transform.position).magnitude;
            if (stepsSinceLastSeenPlayer < 450 && distanceToPlayer < meleeCombatRange)
            {
                state = CrossbowmanState.MeleeCombat;
            }
            else if (stepsSinceLastSeenPlayer < 450)
            {
                state = CrossbowmanState.RangedCombat;
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
                    rangedCombatTree.Tick(deltaTime);
                    break;
            }
        }
    }
}