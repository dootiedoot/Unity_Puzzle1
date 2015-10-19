using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
    [SerializeField] private Vector2 currentCoords;
    [SerializeField] private GameObject currentTile;
    private GameObject previousTile;

    public float moveDuration;
    [SerializeField] private int moveDistance;

    private bool isMoving = false;

    private MapGenerator _map;

    void Awake()
    {
        _map = GameObject.FindWithTag("MapGenerator").GetComponent<MapGenerator>();
    }

    void Start()
    {
        // Initial entity-to-tile setup
        currentCoords = new Vector2(transform.position.x, transform.position.z);
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
        currentCoords = new Vector2(transform.position.x, transform.position.z);
        foreach (GameObject tile in _map.Tiles)
        {
            Tile _tile = tile.GetComponent<Tile>();
            if (_tile.TileCoord == currentCoords)
            {
                currentTile = tile;
                _tile.CurrentTileEnitity = gameObject;
                previousTile.GetComponent<Tile>().CurrentTileEnitity = null;
                previousTile = currentTile;
                break;
            }
        }
    }

    public void ShowMoveTiles()
    {

    }

    // public method to perform Move action
    public void Move(Vector3 direction)
    {
        if (!isMoving)
            StartCoroutine(Move(transform, direction, moveDistance, moveDuration));
    }

    IEnumerator Move(Transform source, Vector3 direction, int distance, float overTime)
    {
        for(int i = 0; i < distance; i++)
        {
            isMoving = true;
            transform.rotation = Quaternion.LookRotation(direction);
            Vector3 newPosition = source.position + direction;
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
        }
        Debug.Log(gameObject.name + " finished move in " + Time.time + " seconds");
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
