using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{
    private bool hasSelected = false;
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
                // if the hit object is a tile and doesn't have a entity on it
                if (hit.collider.CompareTag("Tile") && !hit.collider.GetComponent<Tile>().CurrentTileEnitity)
                {
                    ClearSelections();
                    GameObject[] adjacentTiles = getAdjacentTiles(hit.collider.GetComponent<Tile>());
                    if (!hasSelected)
                    {
                        showTileSelected(adjacentTiles);
                        hasSelected = true;
                    }
                    else
                    {
                        doEntityAction(adjacentTiles);
                        hasSelected = false;
                    }
                }
            }
        }
    }

    public GameObject[] getAdjacentTiles(Tile tile)
    {
        GameObject[] adjacentTiles = new GameObject[4];
        adjacentTiles[0] = tile.TopTile;
        adjacentTiles[1] = tile.RightTile;
        adjacentTiles[2] = tile.BottomTile;
        adjacentTiles[3] = tile.LeftTile;
        return adjacentTiles;
    }

    void ClearSelections()
    {
        foreach (GameObject tile in _map.Tiles)
            tile.GetComponent<Tile>().isSelected = false;
    }

    void showTileSelected(GameObject[] adjacentTiles)
    {
        for (int i = 0; i < 4; i++)
        {
            if (adjacentTiles[i] != null)
            {
                Tile _tile = adjacentTiles[i].GetComponent<Tile>();

                //Visual
                _tile.FlashColor();
            }
        }
    }

    void doEntityAction(GameObject[] adjacentTiles)
    {
        for (int i = 0; i < 4; i++)
        {
            if (adjacentTiles[i] != null)
            {
                Tile _tile = adjacentTiles[i].GetComponent<Tile>();
                _tile.isSelected = false;

                if (i == 0)
                    _tile.EntityAction(Vector3.forward);
                else if (i == 1)
                    _tile.EntityAction(Vector3.right);
                else if (i == 2)
                    _tile.EntityAction(Vector3.back);
                else if (i == 3)
                    _tile.EntityAction(Vector3.left);
            }
        }
    }
}
