using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private Vector2 currentCoords;
    [SerializeField]
    private Tile currentTile;

    public enum Type { Gold, Silver, Bronze };
    public Type acceptedType;

    void OnEnable()
    {
        EntityMotor.OnAction += CheckLock;
    }

    void OnDisable()
    {
        EntityMotor.OnAction -= CheckLock;
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

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckLock()
    {
        if (currentTile.TileEnitities.Count != 0 && currentTile.ContainsEntityTag(Tags.Interactive))
        {
            GameObject entity = currentTile.GetEntityByTag(Tags.Interactive);
            if ((int)entity.GetComponent<EntityInteractive>().myType == (int)acceptedType)
            {
                currentTile.RemoveEntity(entity);
                currentTile.RemoveEntity(gameObject);
                Destroy(entity);
                Destroy(gameObject);
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
