using UnityEngine;
using System.Collections;

public class EntityMotor : MonoBehaviour
{
    [SerializeField] private Vector2 currentCoords;
    [SerializeField] private GameObject currentTile;
    private GameObject previousTile;

    public float moveDuration;
    [SerializeField] private int moveDistance;
    [SerializeField] private int moveAmount = 0;

    private bool isMoving = false;

    private MapGenerator _map;

    void Awake()
    {
        _map = GameObject.FindWithTag("MapGenerator").GetComponent<MapGenerator>();
    }

    void Start()
    {
        // Initial entity-to-tile setup
        currentCoords = new Vector2((int)transform.position.x, (int)transform.position.z);
        transform.position = new Vector3(currentCoords.x, transform.position.y, currentCoords.y);   // Lock transform into int coordinates
        foreach (GameObject tile in _map.Tiles)
        {
            Tile _tile = tile.GetComponent<Tile>();
            if (_tile.TileCoord == currentCoords)
            {
                currentTile = tile;
                _tile.CurrentTileEnitity = gameObject;
                previousTile = currentTile;
                break;
            }
        }
    }

    // Rescan grid to update entity-to-tile data
    void UpdatePosition()
    {
        currentCoords = new Vector2((int)transform.position.x, (int)transform.position.z);
        transform.position = new Vector3(currentCoords.x, transform.position.y, currentCoords.y);   // Lock transform into int coordinates
        foreach (GameObject tile in _map.Tiles)
        {
            Tile _tile = tile.GetComponent<Tile>();
            if (_tile.TileCoord == currentCoords)
            {
                currentTile = tile;
                _tile.CurrentTileEnitity = gameObject;
                if(previousTile != currentTile)
                    previousTile.GetComponent<Tile>().CurrentTileEnitity = null;
                previousTile = currentTile;
                break;
            }
        }
    }

    public void ShowMoveTiles()
    {

    }

    // Recursion that Calculate how many tiles it can possibly move without interference.
    private int GetMoveAmount(Vector3 direction, GameObject currentTile)
    {
        Tile _tile = currentTile.GetComponent<Tile>();
        if(moveAmount < moveDistance && direction == Vector3.forward && _tile.TopTile && !_tile.TopTile.GetComponent<Tile>().CurrentTileEnitity)
        {
            moveAmount++;
            GetMoveAmount(direction, _tile.TopTile);
        }
        else if(moveAmount < moveDistance && direction == Vector3.right && _tile.RightTile && !_tile.RightTile.GetComponent<Tile>().CurrentTileEnitity)
        {
            moveAmount++;
            GetMoveAmount(direction, _tile.RightTile);
        }
        else if(moveAmount < moveDistance && direction == Vector3.back && _tile.BottomTile && !_tile.BottomTile.GetComponent<Tile>().CurrentTileEnitity)
        {
             moveAmount++;
            GetMoveAmount(direction, _tile.BottomTile);
        }
        else if(moveAmount < moveDistance && direction == Vector3.left && _tile.LeftTile && !_tile.LeftTile.GetComponent<Tile>().CurrentTileEnitity)
        {
            moveAmount++;
            GetMoveAmount(direction, _tile.LeftTile);
        }
        //Debug.Log("Movement Check");
        return moveAmount;
    }

    // public method to perform Move action
    public void Move(Vector3 direction)
    {
        if (!isMoving)
        {
            moveAmount = 0;
            StartCoroutine(Move(transform, direction, GetMoveAmount(direction, currentTile), moveDuration));
        }
    }

    IEnumerator Move(Transform source, Vector3 direction, int distance, float overTime)
    {
        isMoving = true;
        transform.rotation = Quaternion.LookRotation(direction);
        Vector3 newPosition = source.position + direction * distance;
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            source.position = Vector3.Lerp(source.position, newPosition, (Time.time - startTime) / overTime);
            if (source.position == newPosition)
                break;
            yield return null;
        }
        source.position = newPosition;
        UpdatePosition();
        isMoving = false;
        //Debug.Log(gameObject.name + " finished move in " + Time.time + " seconds");
    }

    // Accessors and Mutators
    public GameObject CurrentTile
    {
        get { return currentTile; }
        set { currentTile = value; }
    }
    public bool IsMoving
    {
        get { return isMoving; }
        set { isMoving = value; }
    }
    public int GetMoveDistance
    {
        get { return moveDistance; }
        set { moveDistance = value; }
    }
}
