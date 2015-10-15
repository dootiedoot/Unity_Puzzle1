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
                    GameObject[] adjacentTiles = SelectTile(hit.collider.GetComponent<Tile>());
                    EntityAction(adjacentTiles);
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

    public void EntityAction(GameObject[] adjacentTiles)
    {
        //Visual
        _map.ClearMaterials();

        for (int i = 0; i < 4; i++)
        {
            if (adjacentTiles[i] != null)
            {
                Tile _tile = adjacentTiles[i].GetComponent<Tile>();

                if (i == 0)
                    _tile.EntityAction(Vector3.forward);
                else if (i == 1)
                    _tile.EntityAction(Vector3.right);
                else if (i == 2)
                    _tile.EntityAction(Vector3.back);
                else if (i == 3)
                    _tile.EntityAction(Vector3.left);
                   
                _tile.SwapMaterial(1);
            }
        }
    }
}
