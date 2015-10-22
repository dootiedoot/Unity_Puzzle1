using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private Vector2 currentCoords;
    [SerializeField]
    private GameObject currentTile;

    private MapGenerator _map;

    void Awake()
    {
        _map = GameObject.FindWithTag("MapGenerator").GetComponent<MapGenerator>();
    }
    // Use this for initialization
    void Start ()
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
                break;
            }
        }
    }

    public void Die()
    {
        Destroy(gameObject);
        currentTile.GetComponent<Tile>().CurrentTileEnitity = null;
    }
}
