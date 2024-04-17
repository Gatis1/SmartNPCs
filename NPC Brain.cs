using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NPCBrain;

public class NPCBrain : MonoBehaviour
{
    public enum NPCState { Supervised, Unsupervised, Check, Relay }

    public NPCState currentState;
    public bool isSupervised = false;
    int gridWidth = 11;
    int gridHeight = 8;

    public class Node
    {
        public int xPos, yPos;
        public float gCost;
        public float hCost;
        public float fCost { get { return gCost + hCost; } }

        public Node parent;

        public Node(int x, int y)
        {
            this.xPos = x;
            this.yPos = y;
        }
    }

    public class AStar
    {
        private List<Node> GetNeighbors(Node node)
        {
            List<Node> neighbors = new List<Node>();

            int[] dXAxis = { 1, -1, 0, 0 };
            int[] dYAxis = { 0, 0, 1, -1 };

            // Iterate through possible neighbor positions
            for (int i = 0; i < dXAxis.Length; i++)
            {
                int newX = node.x + dXAxis[i];
                int newY = node.y + dYAxis[i];

                // Check if the new position is within the grid bounds
                // and if the cell is traversable (not blocked)
                if (IsValidPosition(newX, newY) && IsTraversable(newX, newY))
                {
                    // Create a new node for the neighbor and add it to the list
                    Node neighbor = new Node(newX, newY);
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }
    }

    private bool IsValidPosition(int x, int y)
    {
        // Check if the position is within the grid bounds
        return x >= 0 && x < gridWidth && y >= 0 && y < gridHeight;
    }

    private bool IsTraversable(int x, int y)
    {
        if (IsValidPosition(x, y))
        {
            // Check if the cell is traversable
            // Modify this logic based on your grid representation
            return grid[x, y] == 0; // Assuming 0 represents a traversable cell
        }
        else
        {
            // If the position is outside the grid bounds, consider it non-traversable
            return false;
        }
    }

    private float CalculateDistance(Node a, Node b)
    {
        int dX = Mathf.abs(a.xPos - b.xPos);
        int dY = Mathf.abs(b.yPos - a.yPos);

        if (dX > dY)
        {
            return 14 * dY + 10 * (dX - dY);
        }
        return 14 * dX + 10 * (dY - dX);
    }

    public List<Node> FindPath(Node startNode, Node targetNode)
    {
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||
                   (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                        currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (!closedSet.Contains(neighbor))
                {
                    float newMovementCostToNeighbor = currentNode.gCost + CalculateDistance(currentNode, neighbor);
                    if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newMovementCostToNeighbor;
                        neighbor.hCost = CalculateDistance(neighbor, targetNode);
                        neighbor.parent = currentNode;

                        if (!openSet.Contains(neighbor))
                        {
                           openSet.Add(neighbor);
                        }
                    }
                }
            }
        }
        // If no path is found, return an empty list
        return new List<Node>();
    }

    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    void Update()
    {
        switch (currentState)
        {
            case NPCState.Supervised:
                if (isSupervised)
                {
                    SearchFortress();
                }
                else
                {
                    // Handle behavior for being in Supervised state but not supervised
                }
                break;
            case NPCState.Unsupervised:
                Wander();
                break;
            case NPCState.Check:
                CheckFortress();
                break;
            case NPCState.Relay:
                RelayInstructions();
                break;
        }
    }

    // Implement A* search to find the nearest fortress
    // If the NPC reaches a fortress, switch to Check state
    void SearchFortress()
    {
        // Define start node as the NPC's current position
        Node startNode = new Node(/* NPC's current position */);

        // Determine the position of the nearest fortress
        Node fortressNode = FindNearestFortress(startNode);

        // Use A* algorithm to find the shortest path to the fortress
        AStar aStar = new AStar();
        List<Node> path = aStar.FindPath(startNode, fortressNode);

        // Move the NPC along the path
        MoveNPCAlongPath(path);

        // Check if the NPC has reached the fortress
        if (HasReachedFortress(path))
        {
            // Switch to the Check state
            currentState = NPCState.Check;
        }
    }

    // Function to find the position of the nearest fortress
    Node FindNearestFortress(Node startNode)
    {
        // Initialize variables to store the nearest fortress position and its distance
        Node nearestFortress = null;
        float nearestDistance = float.MaxValue;

        // Iterate through the list of fortress positions
        foreach (Node fortress in fortressPositions)
        {
            // Calculate the distance between the NPC's current position and the fortress position
            float distance = CalculateDistance(startNode, fortress);

            // Check if the current fortress is closer than the previously found nearest fortress
            if (distance < nearestDistance)
            {
                nearestFortress = fortress;
                nearestDistance = distance;
            }
        }

        return nearestFortress;
    }

    // Function to move the NPC along the path
    void MoveNPCAlongPath(List<Node> path)
    {
        // Iterate through the nodes in the path starting from the second node
        for (int i = 1; i < path.Count; i++)
        {
            // Move the NPC from the current node to the next node in the path
            MoveNPC(path[i]);
        }
    }

    // Function to move the NPC from one node to another
    void MoveNPC(Node destination)
    {
        // Implement logic to move the NPC from its current position to the destination position
        // This might involve setting the NPC's position to the destination position

        // Update NPCPosition to the destination position
        NPCPosition = destination;
        NPCGameObject.transform.position = new Vector3(destination.x, destination.y, 0);
    }

    // Function to check if the NPC has reached the fortress
    bool HasReachedFortress(List<Node> path)
    {
        // Check if the NPC has reached the fortress by comparing its current position
        // with the position of the last node in the path (which should be the fortress position)
        return NPCPosition == path[path.Count - 1];
    }

    void CheckFortress()
    {
        // If it can unlock it, switch to Relay state
        // Otherwise, switch back to Supervised or Unsupervised state based on isSupervised variable
        // Check if the NPC can unlock the fortress (this is a placeholder condition)
        bool canUnlockFortress = CanUnlockFortress();

        if (canUnlockFortress)
        {
            // Switch to Relay state
            currentState = NPCState.Relay;
        }
        else
        {
            // Switch back to Supervised or Unsupervised state based on isSupervised variable
            if (isSupervised)
            {
                currentState = NPCState.Supervised;
            }
            else
            {
                currentState = NPCState.Unsupervised;
            }
        }
    }

    void RelayInstructions()
    {
        // Tell other NPCs where they should go based on the unlocked fortress
        // Get a reference to all other NPCs in the game
        NPC[] otherNPCs = GetAllOtherNPCs();

        // Iterate through each NPC
        foreach (NPC npc in otherNPCs)
        {
            // Check if the NPC is within range or can receive instructions
            if (IsWithinRange(npc))
            {
                // Instruct the NPC on where to go based on the unlocked fortress
                npc.SetDestination(unlockedFortressPosition);
            }
        }
    }

    // Function to get a reference to all other NPCs in the game
    NPC[] GetAllOtherNPCs()
    {
        // Assuming NPCs are tagged as "NPC" in Unity
        GameObject[] npcObjects = GameObject.FindGameObjectsWithTag("NPC");
        List<NPC> otherNPCs = new List<NPC>();

        // Iterate through found NPC GameObjects
        foreach (GameObject npcObject in npcObjects)
        {
            // Exclude the current NPC (this GameObject)
            if (npcObject != gameObject)
            {
                // Get the NPC component from the GameObject and add it to the list
                NPC npcComponent = npcObject.GetComponent<NPC>();
                if (npcComponent != null)
                {
                    otherNPCs.Add(npcComponent);
                }
            }
        }

        return otherNPCs.ToArray();
    }

    // Function to check if an NPC is within range or can receive instructions
    bool IsWithinRange(NPC npc)
    {
        // Assuming there's a maximum range for relaying instructions
        float maxRange = 10f; // Adjust this value according to your game's needs

        // Calculate the distance between this NPC and the target NPC
        float distance = Vector3.Distance(transform.position, npc.transform.position);

        // Check if the distance is within the maximum range
        return distance <= maxRange;
    }

    // Function to set the destination for an NPC
    void SetDestination(Vector3 destination)
    {
        // Assuming the NPC uses Unity's NavMeshAgent for pathfinding
        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();

        // Set the destination for the NavMeshAgent
        navMeshAgent.SetDestination(destination);
    }

    void Wander()
    {
        // Implement random wandering behavior until finding a fortress
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 newPosition = transform.position + new Vector3(randomDirection.x, randomDirection.y, 0);
        // Move NPC to the new position (assuming Unity)
        transform.position = newPosition;
    }
}
