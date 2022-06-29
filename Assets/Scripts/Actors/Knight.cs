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

        [SerializeField] private Material patrolMaterial, combatMaterial;

        private int stepsSinceLastSeenPlayer = 300;
        private Node patrolTree, meleeCombatTree;
        private KnightState state;
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
        }

        public void Sense()
        {
            var player = GameObject.FindWithTag("Player");
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
            if(stepsSinceLastSeenPlayer < 100)
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

                    break;
            }
        }
    }
}