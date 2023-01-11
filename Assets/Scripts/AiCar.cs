using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiCar : MonoBehaviour
{
    public  Vector2[] waypoints;

    public float speed = 10f;
    private AStarGrid grid;
    private AStarTile currentTile;


    private int waypointIndex = 0;

    private List<AStarTile> path = new List<AStarTile>();

    private int elapsedFrames = 0;

    private int interpolationFramesCount = 15;


    


    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<AStarGrid>();
        currentTile = grid.GetTile(transform.position);
        waypoints = new Vector2[2]{ new Vector2(20,20) , new Vector2(21, 26) };
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
                transform.position = MoveToNextTile(path[0]);
            }
        }
    }

    private Vector3 MoveToNextTile(AStarTile tile)
    {
        var step = speed * Time.deltaTime;
        Vector3 interpolatedPosition = Vector3.MoveTowards(transform.position, tile.transform.position, step);
        if(interpolatedPosition == tile.transform.position)
        {
            ReachDestination(tile);
        }
        return interpolatedPosition;
    }

    private void ReachDestination(AStarTile tile)
    {
        elapsedFrames = 0;
        currentTile = tile;  
    }


    private void SetPath()
    {
        print("waypoint index: " + waypointIndex);
        path = grid.GetPath(currentTile, grid.GetTile(waypoints[waypointIndex]));
        waypointIndex = (waypointIndex + 1) % waypoints.Length;
    }
}
