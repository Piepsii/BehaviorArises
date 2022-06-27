using System.Collections.Generic;
using UnityEngine;
using BehaviorArises.Actors;
using BehaviorArises.BehaviorTree;

namespace BehaviorArises{

    public class Main : MonoBehaviour {

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
                pleb.Build();
                plebs.Add(pleb);
            }
        }   

        private void Update(){
            float deltaTime = Time.deltaTime;
            foreach(Pleb pleb in plebs){
                pleb.Tick(deltaTime);
            }
        }     
    }
}