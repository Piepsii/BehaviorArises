using System.Collections.Generic;
using UnityEngine;
using BehaviorArises.Actors;
using BehaviorArises.BehaviorTree;

namespace BehaviorArises{

    public class Main : MonoBehaviour {

        public List<Transform> waypoints;

        public int senseDecideFrequency = 10;
        private int senseDecideCounter = 0;

        public int plebAmount;
        public int knightAmount;
        public int crossbowmanAmount;

        public Transform plebSpawnpoint;
        public Transform knightSpawnpoint;
        public Transform crossbowmanSpawnpoint;

        public GameObject plebPrefab;
        public GameObject knightPrefab;
        public GameObject crossbowmanPrefab;

        private List<Pleb> plebs;
        private List<Knight> knights;
        private List<Crossbowman> crossbowmen;

        private void Start(){
            plebs = new List<Pleb>();
            knights = new List<Knight>();
            crossbowmen = new List<Crossbowman>();

            for(int i = 0; i < plebAmount; i++){
                var instantiatedPleb = Instantiate(plebPrefab, plebSpawnpoint);
                Pleb pleb;
                if(!instantiatedPleb.TryGetComponent<Pleb>(out pleb)){
                    pleb = instantiatedPleb.AddComponent<Pleb>();
                }
                Path path;
                if(!instantiatedPleb.TryGetComponent<Path>(out path)){
                    path = instantiatedPleb.AddComponent<Path>();
                }
                path.waypoints = waypoints;
                pleb.Build();
                plebs.Add(pleb);
            }
        }   

        private void Update(){
            senseDecideCounter++;
            if(senseDecideCounter == senseDecideFrequency)
            {
                senseDecideCounter = 0;
                foreach (Pleb pleb in plebs)
                {
                    pleb.Sense();
                    pleb.Decide();
                }
            }

            float deltaTime = Time.deltaTime;
            foreach(Pleb pleb in plebs){
                pleb.Tick(deltaTime);
            }
        }     
    }
}