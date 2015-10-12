using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour
{
    // VARIABLES
	public Transform tilePrefab;
	public Vector2 mapSize;
    [SerializeField] private GameObject[] tiles;

	[Range(0,1)]
	public float outlinePercent;

	void Awake()
    {
		GenerateMap();
	}

    public void GenerateMap()
    {

        /*string holderName = "Generated Map";
		if (transform.FindChild (holderName))
			DestroyImmediate(transform.FindChild(holderName).gameObject); 

		Transform mapHolder = new GameObject (holderName).transform;
        mapHolder.parent = transform;
        */

        tiles = new GameObject[(int)(mapSize.x * mapSize.y)];
        int totalTiles = 0;

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                //Vector3 tilePosition = new Vector3(-mapSize.x/2 + 0.5f + x, 0, -mapSize.y/2 + 0.5f + y);
                Vector3 tilePosition = new Vector3(x, 0, y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                newTile.localScale = Vector3.one * (1 - outlinePercent);
                //newTile.parent = mapHolder;
                newTile.parent = transform;
                newTile.name = x.ToString() + "," + y.ToString();

                tiles[totalTiles] = newTile.gameObject;
                totalTiles++;
            }
        }

        foreach (GameObject tile in tiles)
        {
            tile.GetComponent<Tile>().Tiles = tiles;
            tile.GetComponent<Tile>().AssignAdjacentTiles();
        }

    }

    void Start()
    {
        
    }

    // Accessors and Mutators
    public GameObject[] Tiles
    {
        get { return tiles; }
        set { tiles = value; }
    }
}
