using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiCar : MonoBehaviour
{
    public  Vector2[] waypoints;
    private AStarGrid grid;
    private AStarTile currentTile;


    private int waypointIndex = 0;

    private List<AStarTile> path = new List<AStarTile>();

    private int elapsedFrames = 0;

    private int interpolationFramesCount = 30;
    


    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<AStarGrid>();
        currentTile = grid.GetTile(transform.position);
        waypoints = new Vector2[2]{ new Vector2(20,20) , new Vector2(24, 21) };
    }

    // Update is called once per frame
    void Update()
    {   
        

        if(path!= null && path.Count > 0)
        {
            FollowPath();
        }
        else
        {
            SetPath();
        }

    }

    private void FollowPath()
    {
        // print(path.Count);
        if (path.Count > 0)
        {
            if (currentTile == path[0])
            {
                path.RemoveAt(0);
            }
            else
            {
                transform.position = LerpToTile(path[0]);
            }
        }
    }

    private Vector3 LerpToTile(AStarTile tile)
    {
        float interpolationRatio = (float)elapsedFrames / interpolationFramesCount;
        Vector3 interpolatedPosition = Vector3.Lerp(transform.position, tile.transform.position, interpolationRatio);
        if(elapsedFrames == interpolationFramesCount+1)
        {
            ReachDestination(tile);
            return tile.transform.position;
        }
        elapsedFrames = (elapsedFrames + 1);
        return interpolatedPosition;
    }

    private void ReachDestination(AStarTile tile)
    {
        elapsedFrames = 0;
        currentTile = tile;  
    }


    private void SetPath()
    {
        
        path = grid.GetPath(currentTile, grid.GetTile(waypoints[waypointIndex]));
        if(path.Count <= 2){
        print(waypointIndex);
        waypointIndex = (waypointIndex + 1) % waypoints.Length;
        print(waypointIndex);
        path = grid.GetPath(currentTile, grid.GetTile(waypoints[waypointIndex]));
        print(path.Count);
        }
    }
}
