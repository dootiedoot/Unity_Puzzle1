using UnityEngine;
using System.Collections;

public class Pawn : MonoBehaviour
{
    [SerializeField] private Vector2 currentCoords;
    [SerializeField] private GameObject currentTile;
    private GameObject previousTile;

    public float moveDuration;
    public int moveDistance;

    private bool isMoving = false;

    private MapGenerator _map;

    void Awake()
    {
        _map = GameObject.FindWithTag("MapGenerator").GetComponent<MapGenerator>();
    }

    void Start()
    {
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

    public void Move(Vector3 direction)
    {
        if (!isMoving)
            StartCoroutine(Move(transform, direction, moveDistance, moveDuration));
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
            yield return null;
        }
        source.position = newPosition;
        UpdatePosition();
        isMoving = false;
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
}
