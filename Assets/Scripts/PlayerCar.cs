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
    // private int elapsedFrames = 0;
    // private int interpolationFramesCount = 30;
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
            storedDirection = Vector3.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            storedDirection = Vector3.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            storedDirection = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            storedDirection = Vector3.right;
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
    // private Vector3 MoveToNextTile(AStarTile tile)
    // {
    //     float interpolationRatio = (float)elapsedFrames / interpolationFramesCount;
    //     Vector3 interpolatedPosition = Vector3.Lerp(transform.position, tile.transform.position, interpolationRatio);
    //     if(elapsedFrames == interpolationFramesCount+1)
    //     {
    //         ReachDestination(tile);
    //         return tile.transform.position;
    //     }
    //     elapsedFrames = (elapsedFrames + 1);
    //     return interpolatedPosition;
    // }
    private void ReachDestination(AStarTile tile)
    {
        nextTile = null;
        currentTile = tile;  
        if (storedDirectionToBeConsumed){
            if(storedDirection != Vector3.zero){
                currentDirection = storedDirection;
            }
            storedDirection = Vector3.zero;
            storedDirectionToBeConsumed = false;
            }
    }
}
