using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private Vector2 currentCoords;
    [SerializeField]
    private GameObject currentTile;

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

    void CheckSubject()
    {
        Debug.Log("Check!");
        if(currentTile.GetComponent<Tile>().TileEnitities.Count != 0)
        {
            /*
            GameObject entity = currentTile.GetComponent<Tile>().CurrentTileEnitity;
            if(entity.GetComponent<EntityType>())
            {
                EntityType _entityType = entity.GetComponent<EntityType>();
                if ((int)_entityType.myType == (int)acceptedType)
                {
                    Debug.Log("Goal!");
                    currentTile.GetComponent<Tile>().CurrentTileEnitity = null;
                    Destroy(entity);
                }
            }*/
        }
    }
}
