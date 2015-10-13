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

                tiles[totalTiles] = newTile.gameObject;
                totalTiles++;
            }
        }
    }

    void Start()
    {
       AssignAdjacentTiles();
    }

    public void AssignAdjacentTiles()
    {
        foreach (GameObject tile in tiles)
        {
            Tile _tile = tile.GetComponent<Tile>();
            Vector2 top     = new Vector2(_tile.TileCoord.x, _tile.TileCoord.y + 1);
            Vector2 right   = new Vector2(_tile.TileCoord.x + 1, _tile.TileCoord.y);
            Vector2 bottom  = new Vector2(_tile.TileCoord.x, _tile.TileCoord.y - 1);
            Vector2 left    = new Vector2(_tile.TileCoord.x - 1, _tile.TileCoord.y);

            //print( _tile.name + top + " " + right + " " + bottom + " " +left);
            foreach (GameObject localTile in tiles)
            {
                //print(localTile.name);
                Tile _localTile = localTile.GetComponent<Tile>();
                
                if (_localTile.TileCoord == top)
                    _tile.TopTile = localTile;
                else if (_localTile.TileCoord == right)
                    _tile.RightTile = localTile;
                else if (_localTile.TileCoord == bottom)
                    _tile.BottomTile = localTile;
                else if (_localTile.TileCoord == left)
                    _tile.LeftTile = localTile;
            }
        }
    }

    public void ClearMaterials()
    {
        foreach (GameObject tile in tiles)
            tile.GetComponent<Tile>().SwapMaterial(0);
    }

    // Accessors and Mutators
    public GameObject[] Tiles
    {
        get { return tiles; }
        set { tiles = value; }
    }
}
