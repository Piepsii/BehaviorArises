# BehaviorArises
Uppsala University - 5SD806 - AI Programming - Assignment 3

Goal: 
The goal of this assignment is to model a complex behaviour between three behaviourally distinct agents using behaviour trees.
A behaviour tree is a mathematical model for GOAP (goal oriented action planning) and is commonly used in robotics.
The agents that I chose to model are all based on "Elden Ring" (2022, "From Software") and will interact with a player character.

Agents: 

![Pleb Image](/BehaviourArises/readme/pleb.png?raw=true "Pleb")
1. Pleb
The pleb is an unarmored companion of the knight. 
They will charge towards the player on sight. 
While patrolling, they will always walk in front of the knight.

2. Knight
The knight is an armored enemy. 
They will patrol an area until they see an enemy upon which combat ensues.
While the plebs are still alive, they will keep their distance with their guard up.
If they are a certain distance from the player, they will use their crossbow.
If the plebs died, they will head towards the player to engage close combat.

3. Bird
