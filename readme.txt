Course Name: AI Programming 1
Course Code: 5SD806
Assignment: Behavior Arises (3)
Student: Paul Brandstetter

Controls:
	Move: WASD
	Camera: mouse
	Jump: space
	Attack: left mouse button

Description:
Create a simulation with three different agents with AI that is implemented using behaviour trees.
The agents should interact with the other agents and the environment in an interesting way.

Notes from the student:
The student chose to model three agents from From Software's Elden Ring.
The agents react based on the players and inherent variables.
Each agent has multiple behavior trees governing a certain part of their behavior.
The agents switch between those behaviors using a finite state machine.
A blackboard helps pass generic data to behavior tree nodes.
Some of the variables are serialized as they only help display the behavior.
All logic related variables are set in code, to help follow the structure.
The agents are built and governed from Main.cs using Sense/Decide/Act principles.