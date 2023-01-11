using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarGrid : MonoBehaviour
{
    public AStarTile[,] grid;
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float tileWidth = 1f;
    public float tileHeight = 1f;
    public float gridOffsetX = 0f;
    public float gridOffsetY = 0f;
    public GameObject tilePrefab;

    private AStarTile startTile;
    private AStarTile endTile;


    private void Awake() {
        
        gridOffsetX = (gridWidth * tileWidth / -2.0f ) + 1.0f;
        gridOffsetY = (gridHeight * tileHeight / -2.0f) + 1.0f;
        CreateGrid();

    }


    public void CreateGrid()
    {
        grid = new AStarTile[gridWidth, gridHeight];
        var rayOffset = new Vector3(0.0f,10.0f,0.0f); 
        var wallMask = LayerMask.GetMask("Walls"); 
        
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                GameObject newTile = Instantiate(tilePrefab, new Vector3(x * tileWidth + gridOffsetX,0, y * tileHeight + gridOffsetY), Quaternion.identity);
                newTile.transform.parent = transform;
                AStarTile tile = newTile.GetComponent<AStarTile>();
                tile.x = x;
                tile.y = y;
                grid[x, y] = tile;
                if(Physics.Raycast(newTile.transform.position + rayOffset,  Vector3.down, Mathf.Infinity, wallMask))
                {
                    SetWallTile(tile);
                }
            }
        }
        for (var x = 0; x < gridWidth; x++)
        {
            for (var y = 0; y < gridHeight; y++)
            {
                grid[x, y].SetNeighbours(GetNeighbours(grid[x, y]));
            }
        }
    }


    public AStarTile GetRandomTile()
    {
        int x = Random.Range(0, gridWidth);
        int y = Random.Range(0, gridHeight);
        if (grid[x, y].isWalkable)
        {
            return grid[x, y];
        }
        else
        {
            return GetRandomTile();
        }
    }



    public AStarTile GetTile(int x, int y)
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        { 
            return grid[x, y];
        }
        else
        {
            return null;
        }
    }
    

    public AStarTile GetTile(Vector3 position)
    {
        int x = Mathf.FloorToInt((position.x - gridOffsetX) / tileWidth);
        int y = Mathf.FloorToInt((position.z - gridOffsetY) / tileHeight);
        return GetTile(x, y);
    }
    public AStarTile GetTile(Vector2 position)
    {
        int x = Mathf.FloorToInt((position.x - gridOffsetX) / tileWidth);
        int y = Mathf.FloorToInt((position.y - gridOffsetY) / tileHeight);
        return GetTile(x, y);
    }

    public List<AStarTile> GetNeighbours(AStarTile tile)
    {
        List<AStarTile> neighbours = new List<AStarTile>();
        Vector2[] dirs = {new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0)}; 
        foreach (var dir in dirs)
        {
            var neighbour = GetTile(tile.x + (int) dir.x, tile.y + (int) dir.y);
            if (neighbour != null)
                neighbours.Add(neighbour);
        }
        return neighbours;
    }


    public AStarTile GetNeighbour(AStarTile tile, Vector2 dir)
    {
        foreach (var neighbour in tile.neighbours)
        {
            if (neighbour.x == tile.x + (int) dir.x && neighbour.y == tile.y + (int) dir.y)
            {
                return neighbour;
            }
        }
        return null;
    }


    
    public List<AStarTile> GetPath(AStarTile start, AStarTile end)
    {
        foreach (var tile in grid)
        {
            tile.ResetTile();
        }
        List<AStarTile> openList = new List<AStarTile>();
        List<AStarTile> closedList = new List<AStarTile>();
        openList.Add(start);
        while (openList.Count > 0)
        {
            AStarTile current = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < current.fCost || openList[i].fCost == current.fCost && openList[i].hCost < current.hCost)
                {
                    current = openList[i];
                }
            }
            openList.Remove(current);
            closedList.Add(current);
            if (current == end)
            {
                return GetFinalPath(start, end);
            }
            foreach (AStarTile neighbour in current.neighbours)
            {
                if (!neighbour.isWalkable || closedList.Contains(neighbour))
                {
                    continue;
                }
                int newMovementCostToNeighbour = current.gCost + GetDistance(current, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openList.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, end);
                    neighbour.parent = current;
                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }
        return null;
    }

    public List<AStarTile> GetFinalPath(AStarTile start, AStarTile end)
    {
        List<AStarTile> finalPath = new List<AStarTile>();
        AStarTile current = end;
        while (current != start)
        {
            finalPath.Add(current);
            current = current.parent;
        }
        finalPath.Reverse();
        return finalPath;
    }

    public int GetDistance(AStarTile tileA, AStarTile tileB)
    {
        int distanceX = Mathf.Abs(tileA.x - tileB.x);
        int distanceY = Mathf.Abs(tileA.y - tileB.y);
        if (distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        return 14 * distanceX + 10 * (distanceY - distanceX);
    }


    public void SetWallTile(AStarTile tile)
    {
        tile.isWall = true;
        tile.isWalkable = false;
        tile.SetTileColor(Color.black);
    }



}
