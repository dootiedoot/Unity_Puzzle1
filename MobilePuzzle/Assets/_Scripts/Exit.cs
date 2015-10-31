using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour
{
    // VARIABLES
    // Tile
    [SerializeField]
    private Vector2 currentCoords;
    [SerializeField]
    private Tile currentTile;

    public enum Type { Red, Blue, Green };
    public Type acceptedType;

    void OnEnable()
    {
        EntityMotor.OnAction += CheckSubject;
    }

    void OnDisable()
    {
        EntityMotor.OnAction -= CheckSubject;
    }

    // Use this for initialization
    void Start()
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

    void CheckSubject()
    {
        //Debug.Log("Check!");
        if (currentTile.TileEnitities.Count != 0 && currentTile.ContainsEntityTag(Tags.Player))
        {
            GameObject entity = currentTile.GetEntityByTag(Tags.Player);
            if ((int)entity.GetComponent<EntityType>().myType == (int)acceptedType)
            {
                Debug.Log("Goal: " + entity.name);
                currentTile.RemoveEntity(entity);
                Destroy(entity);
            }
        }
    }

    // Accessors and Mutators
    public Tile CurrentTile
    {
        get { return currentTile; }
        set { currentTile = value; }
    }
}
