using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{

    private MapGenerator _map;

    void Awake()
    {
        _map = GameObject.FindWithTag("MapGenerator").GetComponent<MapGenerator>();
    }

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Tile"))
                {
                    _map.ClearMaterials();
                    GameObject[] adjacentTiles = SelectTile(hit.collider.GetComponent<Tile>());
                    foreach (GameObject tile in adjacentTiles)
                    {
                        if(tile != null)
                        {
                            Tile _tile = tile.GetComponent<Tile>();
                            _tile.SwapMaterial(1);     
                        }
                    }
                }
            }
        }
    }

    public GameObject[] SelectTile(Tile tile)
    {
        GameObject[] adjacentTiles = new GameObject[4];
        adjacentTiles[0] = tile.TopTile;
        adjacentTiles[1] = tile.RightTile;
        adjacentTiles[2] = tile.BottomTile;
        adjacentTiles[3] = tile.LeftTile;
        return adjacentTiles;
    }
}
