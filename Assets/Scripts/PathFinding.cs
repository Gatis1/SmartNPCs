using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public int xPos;
    public int yPos;
    public int gValue;
    public int hValue;
    public PathNode parentNode;

    public int fValue
    {
        get
        {
            return gValue + hValue;
        }
    }

    public PathNode(int xPos, int yPos)
    {
        this.xPos = xPos;
        this.yPos = yPos;
    }

    internal void Clear()
    {
        gValue = 0;
        hValue = 0;
        parentNode = null;
    }
}

[RequireComponent(typeof(GameManager))]
[RequireComponent(typeof(Tile))]

public class PathFinding : MonoBehaviour
{
    GridManager grid;
    PathNode[,] pathNode;
    Tile tile;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if(grid == null)
        {
            grid = GetComponent<GridManager>();
        }

        pathNode = new PathNode[grid.width, grid.height];

        for(int x = 0; x < grid.width; x++)
        {
            for(int y = 0; y < grid.height; y ++)
            {
                pathNode[x, y] = new PathNode(x, y);
            }
        }
    }

    public void CaculateWalkableTerrian(int starX, int startY, int range ,ref List<PathNode> tohighLight)
    {
        range *= 10;
        PathNode startNode = pathNode[starX, startY];

        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closeList = new List<PathNode>();

        openList.Add(startNode);

        while(openList.Count > 0)
        {
            PathNode currentNode = openList[0];

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            List<PathNode> neighbours = new List<PathNode>();
            for(int x=-1; x<2; x++)
            {
                for(int y=-1; y<2; y++)
                {
                    if(x == 0 && y == 0)
                    {
                        continue;
                    }
                    neighbours.Add(pathNode[currentNode.xPos + x, currentNode.yPos + y]);
                }
            }
            for(int i=0; i<neighbours.Count; i++)
            {
                if(closeList.Contains(neighbours[i]))
                {
                    continue;
                }
                
                if (tile.Walkable == false)
                {
                    closeList.Add(neighbours[i]);
                }

                int moveCost = currentNode.gValue + CalculateDist(currentNode, neighbours[i]);

                if(moveCost > range)
                {
                    continue;
                }

                if(openList.Contains(neighbours[i]) == false || moveCost < neighbours[i].gValue)
                {
                    neighbours[i].gValue = moveCost;
                    neighbours[i].parentNode = currentNode;

                    if(openList.Contains(neighbours[i]) == false)
                    {
                        openList.Add(neighbours[i]);
                    }
                }
            }
        }

        if(tohighLight != null)
        {
            tohighLight.AddRange(closeList);
        }
    }

    internal void Clear()
    {
        for(int x = 0; x < grid.width; x++)
        {
            for(int y = 0; y < grid.height; y++)
            {
                pathNode[x, y].Clear(); 
            }
        }
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = pathNode[startX, startY];
        PathNode endNode = pathNode[endX, endY];

        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closeList = new List<PathNode>();

        openList.Add(startNode);

        while(openList.Count > 0)
        {
            PathNode currentNode = openList[0];

            for (int i = 0; i < openList.Count; i++)
            {
                if(currentNode.fValue > openList[i].fValue)
                {
                    currentNode = openList[i];
                }
                if(currentNode.fValue == openList[i].fValue && currentNode.hValue > openList[i].hValue)
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            if(currentNode == endNode)
            {
                return RetracePath(startNode, endNode);
            }

            List<PathNode> neighbourNode = new List<PathNode>();
            for(int x = -1; x < 2; x++)
            {
                for(int y = -1; y < 2; y++)
                {
                    if(x == 0 && y == 0)
                    {
                        continue;
                    }

                    neighbourNode.Add(pathNode[currentNode.xPos + x, currentNode.yPos + y]);
                }
            }

            for(int i = 0; i < neighbourNode.Count; i++)
            {
                if (closeList.Contains(neighbourNode[i]))
                {
                    continue;
                }

                if (tile.Walkable == false)
                {
                    closeList.Add(neighbourNode[i]);
                }

                int mvCost = currentNode.gValue + CalculateDist(currentNode, neighbourNode[i]);

                if(openList.Contains(neighbourNode[i]) == false || mvCost < neighbourNode[i].gValue)
                {
                    neighbourNode[i].gValue = mvCost;
                    neighbourNode[i].hValue = CalculateDist(neighbourNode[i], endNode);
                    neighbourNode[i].parentNode = currentNode;
                }
                if(openList.Contains(neighbourNode[i]) == false)
                {
                    openList.Add(neighbourNode[i]);
                }
            }
        }
        return null;
    }

    private List<PathNode> RetracePath(PathNode startNode, PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }
        path.Reverse();

        return path;
    }

    private int CalculateDist(PathNode currentNode, PathNode target)
    {
        int distX = Mathf.Abs(currentNode.xPos - target.xPos);
        int distY = Mathf.Abs(currentNode.yPos - target.yPos);

        if(distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        return 14 * distX + 10 * (distY - distX);
    }
}
