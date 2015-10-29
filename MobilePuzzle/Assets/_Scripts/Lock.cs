using UnityEngine;
using System.Collections;

public class Lock : MonoBehaviour
{
    [SerializeField]
    private Vector2 currentCoords;
    [SerializeField]
    private GameObject currentTile;

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
        foreach (GameObject tile in MapGenerator.tiles)
        {
            Tile _tile = tile.GetComponent<Tile>();
            if (_tile.TileCoord == currentCoords)
            {
                currentTile = tile;
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
        Tile _tile = currentTile.GetComponent<Tile>();
        if (_tile.TileEnitities.Count != 0 && _tile.ContainsEntityTag("Interactive"))
        {
            GameObject entity = _tile.GetEntityByTag("Interactive");
            if ((int)entity.GetComponent<EntityInteractive>().myType == (int)acceptedType)
            {
                _tile.RemoveEntity(entity);
                _tile.RemoveEntity(gameObject);
                Destroy(entity);
                Destroy(gameObject);
            }
        }

    }

}
