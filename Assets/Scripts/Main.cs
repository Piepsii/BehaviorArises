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
                if(!instantiatedPleb.TryGetComponent(out pleb)){
                    pleb = instantiatedPleb.AddComponent<Pleb>();
                }
                Path path;
                if(!instantiatedPleb.TryGetComponent(out path)){
                    path = instantiatedPleb.AddComponent<Path>();
                }
                path.waypoints = waypoints;
                pleb.Build();
                plebs.Add(pleb);
            }

            for(int i = 0; i < knightAmount; i++)
            {
                var instantiatedKnight = Instantiate(knightPrefab, knightSpawnpoint);
                Knight knight;
                if(!instantiatedKnight.TryGetComponent(out knight))
                {
                    knight = instantiatedKnight.AddComponent<Knight>();
                }
                Path path;
                if(!instantiatedKnight.TryGetComponent(out path))
                {
                    path = instantiatedKnight.AddComponent<Path>();
                }
                path.waypoints = waypoints;
                GameObject[] plebGOs = new GameObject[plebs.Count];
                for(int j = 0; j < plebs.Count; j++)
                {
                    plebGOs[j] = plebs[j].gameObject;
                }
                knight.Build(plebGOs);
                knights.Add(knight);
            }

            for(int i = 0; i < crossbowmanAmount; i++)
            {
                var instantiatedCrossbowman = Instantiate(crossbowmanPrefab, crossbowmanSpawnpoint);
                Crossbowman crossbowman;
                if(!instantiatedCrossbowman.TryGetComponent(out crossbowman))
                {
                    crossbowman = instantiatedCrossbowman.AddComponent<Crossbowman>();
                }
                Path path;
                if(!instantiatedCrossbowman.TryGetComponent(out path))
                {
                    path = instantiatedCrossbowman.AddComponent<Path>();
                }
                path.waypoints = waypoints;
                crossbowman.Build();
                crossbowmen.Add(crossbowman);
            }
        }   

        private void Update(){
            senseDecideCounter++;
            if(senseDecideCounter == senseDecideFrequency)
            {
                senseDecideCounter = 0;
                foreach (Pleb pleb in plebs)
                {
                    if (pleb == null)
                        continue;
                    pleb.Sense();
                    pleb.Decide();
                }
                foreach(Knight knight in knights)
                {
                    if (knight == null)
                        continue;
                    knight.Sense();
                    knight.Decide();
                }
            }

            float deltaTime = Time.deltaTime;
            foreach(Pleb pleb in plebs)
            {
                if (pleb == null)
                    continue;
                pleb.Tick(deltaTime);
            }
            foreach(Knight knight in knights)
            {
                if (knight == null)
                    continue;
                knight.Tick(deltaTime);
            }
        }     
    }
}