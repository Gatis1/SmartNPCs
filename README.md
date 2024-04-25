# SmartNPCs
CS-450 group project

Team: NPC Brains - Gerome Atis, Joseph Vue, Lucio Beltran, Stephen Schaer, Sahib Singh

How to run the program - Using Unity editor version 2021.3.34f1, the program will already have a single scene where the NPCs and their respective scripts and physics components are appended to them. A canvas is set up to show a background and a timer with a Timer.cs script appended to it for testing purposes. To run the program in the Unity editor there will be a play button towards the top of the scene editor, once pressed the scripts and objects will load and the demo will play and the NPCs will act according to the functions in their scripts.

Items in the program: 
Hunter.cs - script for the hunter
Prey.cs - script for the prey
Timer.cs - script for the timer

Hunter prefab - the hunter object to be placed in the scene and have Hunter.cs appended to
Prey prefab - the prey object to be placed in the scene and have Prey.cs appended to

Description - A unity-made demo to showcase AI in video games. Two Non-Playable Characters (NPC) will be based on Simpe-reflex and Goal-based agents. A hunter NPC will be given the goal of catching a prey NPC as quickly as possible. A prey NPC will be given the goal of evading the hunter NPC before it is allowed to escape.

Original Method - Create a 2D grid and have two NPCs both equipped with an A* search algorithm. A* search is a common algorithm in video games used for pathfinding from one point to another. The idea was to incorporate supervised and unsupervised learning, One NPC, a supervised learner, would use A* search to maneuver the grid and find the goal. In contrast, the other NPC, the unsupervised learner, would traverse the grid in random directions all the while mapping the grid to find the optimal path. This idea was unfortunately dropped due to how complicated it was to set up a grid environment in Unity and was henced swapped to giving NPC autonomy by testing reflex and goal agents.

Experiment - The two NPCs will be placed on opposite ends of the scene and once the scene starts the hunter will start heading towards the current position of the prey. The prey will check how close the hunter is and calculate a new position to traverse to escape the hunter based on a range of angles starting from 0 to 90, 120, and 150. Both NPCs are timed to see how quickly the hunter can catch the prey or how quickly the prey can escape the hunter. The experiment will focus on the hunter tracking its successes and failures. The failures of the hunter will be seen as the successes of the prey and its efficiency in evading the hunter.

Results - Based on the movement ranges of 90 and 120 degrees the Prey was able to escape 7 out of 10 times from the hunter only being caught by the hunter 3 times due to bad turns or being stuck on the corner of the screen. For the 150-degree movement range the Prey was caught all ten times within 1-2 seconds due to it taking too long to calculate a new position to traverse. The hunter, on the other hand, needs a mechanic to "catch" the prey rather than trying to get it on a bad turn or it stuck in the corner of the screen. All-in-all both NPCs worked as aspected reacting to one another's movements but there is room to improve to include more complexity in maneuvering. With more work done the NPCs could do more actions possibly being able to navigate obstacles or use other mechanics.

Resources: Use Unity documentation from the official Unity website to look up specific functions for game design, and while not stated use various YouTube tutorials for setting up the Timer user interface (UI) or camera manipulation.