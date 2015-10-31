using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject selectedTile;
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // if the hit object is a tile and doesn't have a entity on it
                if (hit.collider.CompareTag("Tile") && hit.collider.GetComponent<Tile>().TileEnitities.Count == 0)
                {
                    ClearSelections();
                    GameObject[] adjacentTiles = hit.collider.GetComponent<Tile>().GetAdjacentTiles();

                    /*if (!selectedTile)  
                    {
                        showTileSelected(adjacentTiles);
                        selectedTile = hit.collider.gameObject;
                    }
                    else if(hit.collider.gameObject == selectedTile)
                    {
                        doEntityAction(adjacentTiles);
                        selectedTile = null;
                    }
                    else
                        selectedTile = null;*/

                    showTileSelected(adjacentTiles);
                    if (!GameManager.IsPlayerMoving)
                        doEntityAction(adjacentTiles);
                    selectedTile = null;
                }
                else if (hit.collider.CompareTag("Player"))
                {
                    ClearSelections();
                    //hit.collider.GetComponent<EntityMotor>().ShowMoveTiles();
                    selectedTile = null;
                }
                else if (hit.collider.CompareTag("Destructor") && !GameManager.IsPlayerMoving)
                {
                    hit.collider.GetComponent<Destructor>().Detonate();
                }
            }
        }
    }

    void ClearSelections()
    {
        foreach (GameObject tile in MapGenerator.tiles)
            tile.GetComponent<Tile>().isSelected = false;
    }

    void showTileSelected(GameObject[] adjacentTiles)
    {
        for (int i = 0; i < 5; i++)
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
