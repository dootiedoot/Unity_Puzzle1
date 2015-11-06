using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Tile selectedTile;
	
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
                if (hit.collider.CompareTag(Tags.Tile) && hit.collider.GetComponent<Tile>().TileEnitities.Count == 0)
                {
                    ClearSelections();
                    Tile[] adjacentTiles = hit.collider.GetComponent<Tile>().GetAdjacentTiles();

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
                    if (!GameManager.IsPlayerMoving && GameManager.IsPlayerTurn)
                        doEntityAction(adjacentTiles);
                    selectedTile = hit.collider.GetComponent<Tile>();
                }
                else if (hit.collider.CompareTag(Tags.Player))
                {
                    ClearSelections();
                    //hit.collider.GetComponent<EntityMotor>().ShowMoveTiles();
                    selectedTile = null;
                }
                else if (hit.collider.CompareTag(Tags.Destructor) && !GameManager.IsPlayerMoving)
                {
                    hit.collider.GetComponent<EntityDestructor>().Detonate();
                }
            }
        }
    }

    void ClearSelections()
    {
        foreach (Tile tile in MapGenerator.tiles)
            if(tile.GetComponent<Tile>().isSelected != false)
                tile.GetComponent<Tile>().isSelected = false;
    }

    void showTileSelected(Tile[] adjacentTiles)
    {
        for (int i = 0; i < 5; i++)
        {
            if (adjacentTiles[i] != null)
            {
                Tile _tile = adjacentTiles[i];

                //Visual
                _tile.FlashColor();
            }
        }
    }

    void doEntityAction(Tile[] adjacentTiles)
    {
        for (int i = 0; i < 4; i++)
        {
            if (adjacentTiles[i] != null)
            {
                Tile _tile = adjacentTiles[i];
                _tile.isSelected = false;

                if(_tile.ContainsEntityTag(Tags.Player))
                {
                    EntityMotor _entityMotor = _tile.GetEntityByTag(Tags.Player).GetComponent<EntityMotor>();
                    if (i == 0) // if Toptile
                    {
                        //_entityMotor.MoveAmount = 0;
                        _entityMotor.Move(Vector3.forward);
                    }
                    else if (i == 1) //if RightTile
                    {
                        //_entityMotor.MoveAmount = 0;
                        _entityMotor.Move(Vector3.right);
                    }
                    else if (i == 2) //if BottomTile
                    {
                        //_entityMotor.MoveAmount = 0;
                        _entityMotor.Move(Vector3.back);
                    }
                    else if (i == 3) //if LeftTile
                    {
                        //_entityMotor.MoveAmount = 0;
                        _entityMotor.Move(Vector3.left);
                    }
                }
            }
        }
    }
}
