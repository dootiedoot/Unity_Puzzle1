using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private Vector2 currentCoords;
    [SerializeField]
    private Tile currentTile;

    // Use this for initialization
    void Start ()
    {
        // Initial entity-to-tile setup
        currentCoords = new Vector2((int)transform.position.x, (int)transform.position.z);
        transform.position = new Vector3(currentCoords.x, transform.position.y, currentCoords.y);   // Lock transform into int coordinates
        foreach (Tile _tile in MapGenerator.tiles)
        {
            if (_tile.TileCoord == currentCoords)
            {
                currentTile = _tile;
                _tile.TileEnitities.Add(gameObject);
                break;
            }
        }
    }

    public void Die()
    {
        Destroy(gameObject);
        currentTile.RemoveEntity(gameObject);
    }

    // Accessors and Mutators
    public Tile CurrentTile
    {
        get { return currentTile; }
        set { currentTile = value; }
    }
}
