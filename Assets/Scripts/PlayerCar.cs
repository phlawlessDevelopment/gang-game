using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCar : MonoBehaviour
{
    private AStarGrid grid;
    private AStarTile currentTile;
    private AStarTile nextTile;
    private Vector3 currentDirection = Vector3.right;
    private Vector3 storedDirection = Vector3.up;
    private Vector3 storedDirectionCache = Vector3.zero;
    private float speed = 10f;
    private bool storedDirectionToBeConsumed = false;

    public CinemachineVirtualCamera[] cams;
    private int currentCamIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<AStarGrid>();
        currentTile = grid.GetTile(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        SetNextTile();
        TryMove();
    }


    private void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (storedDirectionToBeConsumed)
            {
                storedDirectionCache = Vector3.up;
            }
            else{
                storedDirection = Vector3.up;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (storedDirectionToBeConsumed)
            {
                storedDirectionCache = Vector3.down;
            }
            else{
                storedDirection = Vector3.down;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (storedDirectionToBeConsumed)
            {
                storedDirectionCache = Vector3.left;
            }
            else{
                storedDirection = Vector3.left;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (storedDirectionToBeConsumed)
            {
                storedDirectionCache = Vector3.right;
            }
            else{
                storedDirection = Vector3.right;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            currentDirection = Vector3.zero;
            storedDirection = Vector3.zero;
            CycleCameras();
        }
    }

    private void CycleCameras()
    {
        cams[currentCamIndex].Priority = 0;
        currentCamIndex = (currentCamIndex + 1) % cams.Length;
        cams[currentCamIndex].Priority = 10;
    }

    public void SetNextTile()
    {   
        if(currentTile == null || (nextTile != null && nextTile.isWalkable))
            return;
        if (storedDirection != Vector3.zero)
        {   
            var tile = grid.GetNeighbour(currentTile, storedDirection);
            if(tile != null && tile.isWalkable)
            {
                nextTile = tile;
                storedDirectionToBeConsumed = true;
                return;
            }
        }
        nextTile = grid.GetNeighbour(currentTile, currentDirection);
    }
    private void TryMove(){

        if (nextTile != null && nextTile.isWalkable)
        {   
            transform.position = MoveToNextTile(nextTile);        
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
        nextTile = null;
        currentTile = tile;  
        if (storedDirectionToBeConsumed){
            if(storedDirection != Vector3.zero){
                currentDirection = storedDirection;
            }
            if (storedDirectionCache != Vector3.zero){
                storedDirection = storedDirectionCache;
                storedDirectionCache = Vector3.zero;
            }
            else{
            storedDirection = Vector3.zero;
            }
            storedDirectionToBeConsumed = false;
            }
    }
}
