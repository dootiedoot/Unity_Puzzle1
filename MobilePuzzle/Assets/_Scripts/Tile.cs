using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    //VARIABLES
    [SerializeField] private Vector2 tileCoord;
    int xCoord;
    int yCoord;

    [SerializeField] private GameObject currentTileEnitity;
    [SerializeField] private GameObject UpTile;
    [SerializeField] private GameObject RightTile;
    [SerializeField] private GameObject DownTile;
    [SerializeField] private GameObject LeftTile;

    private GameObject[] tiles;

    // Use this for initialization
    void Start ()
    {
        string[] coords = name.Split(',');
        xCoord = int.Parse(coords[0]);
        yCoord = int.Parse(coords[1]);
        tileCoord = new Vector2(xCoord, yCoord);
        
    }

    public void AssignAdjacentTiles()
    {
        foreach (GameObject tile in tiles)
        {
            if  (tile.GetComponent<Tile>().TileCoord == new Vector2(tileCoord.x, tileCoord.y + 1))
                UpTile = tile;
            /*else if (tile.GetComponent<Tile>().TileCoord == new Vector2(tileCoord.x + 1, tileCoord.y))
                RightTile = tile;
            else if (tile.GetComponent<Tile>().TileCoord == new Vector2(tileCoord.x, tileCoord.y - 1))
                DownTile = tile;
            else if (tile.GetComponent<Tile>().TileCoord == new Vector2(tileCoord.x - 1, tileCoord.y))
                LeftTile = tile;*/
        }
    }

    // Accessors and Mutators
    public Vector2 TileCoord
    {
        get { return tileCoord; }
        set { tileCoord = value; }
    }
    public GameObject CurrentTileEnitity
    {
        get { return currentTileEnitity; }
        set { currentTileEnitity = value; }
    }

    public GameObject[] Tiles
    {
        get { return tiles; }
        set { tiles = value; }
    }
}
