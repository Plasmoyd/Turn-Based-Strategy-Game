using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    public static Pathfinding Instance { get; private set; }

    [SerializeField] Transform pathfindingDebugObjectTransform;

    private const int DIAGONAL_MOVE_COST = 14;
    private const int STRAIGHT_MOVE_COST = 10;

    private int width = 10;
    private int height = 10;
    private float cellSize = 2f;
    private GridSystem<PathNode> gridSystem;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("Theres more than one3 instance of gameObject : " + transform);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        gridSystem = new GridSystem<PathNode>(width, height, cellSize, (GridSystem<PathNode> gridSystem, GridPosition gridPosition) => new PathNode(gridPosition));
        gridSystem.CreateDebugObjects(pathfindingDebugObjectTransform);
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {

        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startPathNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endPathNode = gridSystem.GetGridObject(endGridPosition);

        for(int x = gridSystem.GetStartingWidth(); x < gridSystem.GetGridWidth(); x++)
        {
            for(int z = gridSystem.GetStartingHeight(); z < gridSystem.GetGridHeight(); z++)
            {
                PathNode pathNode = GetNode(x, z);
                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }

        startPathNode.SetGCost(0);
        startPathNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startPathNode.CalculateFCost();

        openList.Add(startPathNode);

        while(openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);

            if (currentNode == endPathNode)
            {
                //if we are in this check, we have reached our final node, and it's time to calculate a path
                return CalculatePath(endPathNode);
            }

            //if this is not the final node, we want to remove the current node from the open list, and add it to the closed one
            // we also want to search for all neighbouring nodes

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            List<PathNode> neighbourList = GetNeighbours(currentNode);

            foreach(PathNode neighbourNode in neighbourList)
            {

                if(closedList.Contains(neighbourNode))
                {
                    //if the algorithm has already checked this node out, then we should not check it again.
                    continue;
                }

                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

                if(tentativeGCost < neighbourNode.GetGCost())
                {
                    //if this check is true, then there is a better way to get to this node, than the node itself currently thinks there is

                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endPathNode.GetGridPosition()));
                    neighbourNode.CalculateFCost();
                    neighbourNode.SetCameFromPathNode(currentNode);

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        //this happens only if we can't find a path to the end node
        return null;
    }

    private List<GridPosition> CalculatePath(PathNode pathNode)
    {
        List<GridPosition> pathList = new List<GridPosition>();

        pathList.Add(pathNode.GetGridPosition());
        PathNode currentNode = pathNode.GetCameFromPathNode();

        while(currentNode != null)
        {
            pathList.Add(currentNode.GetGridPosition());
            currentNode = currentNode.GetCameFromPathNode();
        }

        pathList.Reverse();
        return pathList;
    }

    private int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        GridPosition gridDistance = gridPositionA - gridPositionB;
        int xDistance = Mathf.Abs(gridDistance.x);
        int zDistance = Mathf.Abs(gridDistance.z);

        int diagonalMoves = Mathf.Min(xDistance, zDistance);
        int straightMoves = Mathf.Abs(xDistance - zDistance);

        return diagonalMoves * DIAGONAL_MOVE_COST + straightMoves * STRAIGHT_MOVE_COST;
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestNode = null;

        foreach(PathNode pathNode in pathNodeList)
        {
            if(lowestNode == null)
            {
                lowestNode = pathNode;
                continue;
            }

            if(pathNode.GetFCost() < lowestNode.GetFCost())
            {
                lowestNode = pathNode;
            }
        }

        return lowestNode;
    }

    private List<PathNode> GetNeighbours(PathNode currentPathNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        GridPosition currentGridPosition = currentPathNode.GetGridPosition();

        for(int x = currentGridPosition.x - 1; x <= currentGridPosition.x + 1; x++)
        {
            for(int z = currentGridPosition.z - 1; z <= currentGridPosition.z + 1; z++)
            {
                if(x < gridSystem.GetStartingWidth() || x >= gridSystem.GetGridWidth())
                {
                    continue;
                }

                if(z < gridSystem.GetStartingHeight() || z >= gridSystem.GetGridHeight())
                {
                    continue;
                }

                neighbourList.Add(GetNode(x, z));
            }
        }

        return neighbourList;
    }

    private PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }
}
