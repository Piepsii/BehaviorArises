# BehaviorArises
Uppsala University - 5SD806 - AI Programming - Assignment 3

Goal: 
The goal of this assignment is to model a complex behaviour between three behaviourally distinct agents using behaviour trees.
A behaviour tree is a mathematical model for GOAP (goal oriented action planning) and is commonly used in robotics.
The agents that I chose to model are all based on "Elden Ring" (2022, "From Software") and will interact with a player character.

Agents: 

![Pleb Image](/readme/Pleb.PNG?raw=true "Pleb")
![Pleb (Elden Ring) Image](/readme/Pleb_ER.PNG?raw=true "Pleb")
1. Pleb
The pleb is an unarmored enemy with a short sword.
They will patrol an area until they see an enemy upon which combat ensues.
They will charge towards the player on sight.

![Knight Image](/readme/Knight.PNG?raw=true "Knight")
![Knight (Elden Ring) Image](/readme/Knight_ER.PNG?raw=true "Knight")
2. Knight
The knight is an armored enemy with sword and shield. 
They will patrol an area until they see an enemy upon which combat ensues.
They will head towards the player to engage close combat.

![Crossbowman Image](/readme/Crossbowman.PNG?raw=true "Crossbowman")
![Crossbowman (Elden Ring) Image](/readme/Crossbowman_ER.PNG?raw=true "Crossbowman")
3. Crossbowman
The crossbow knight is an armored enemy with sword, shield and crossbow.
They will patrol an area until they see an enemy upon which combat ensues.
They will keep their distance.
If they are a certain distance from the player, they will use their crossbow.
If the player is close enough, they will switch to sword and shield.
