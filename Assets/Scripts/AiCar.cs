using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiCar : MonoBehaviour
{
    public  List<Vector3> waypoints = new List<Vector3>();
    public float speed = 10f;
    private AStarGrid grid;
    private AStarTile currentTile;

    private int waypointIndex = 0;

    private List<AStarTile> path = new List<AStarTile>();

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<AStarGrid>();
        currentTile = grid.GetTile(transform.position);
        var allWaypoints = FindObjectsOfType<Waypoint>();
        foreach (var waypoint in allWaypoints)
        {
            if (waypoint.aiCar == this)
            {
                waypoints.Add(waypoint.transform.position);
                waypoint.gameObject.SetActive(false);
            }
        } 
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
        currentTile = tile;  
    }


    private void SetPath()
    {
        path = grid.GetPath(currentTile, grid.GetTile(waypoints[waypointIndex]));
        waypointIndex = (waypointIndex + 1) % waypoints.Count;
    }
}
