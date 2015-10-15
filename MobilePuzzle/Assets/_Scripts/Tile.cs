using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    //VARIABLES
    [SerializeField] private Vector2 tileCoord;
    int xCoord;
    int yCoord;

    [SerializeField] private GameObject currentTileEnitity;
    [SerializeField] private GameObject topTile;
    [SerializeField] private GameObject rightTile;
    [SerializeField] private GameObject bottomTile;
    [SerializeField] private GameObject leftTile;

    // visuals
    public Material defaultMat;
    public Material highlightMat;

    // Use this for initialization
    void Awake ()
    {
        tileCoord = new Vector2(transform.position.x, transform.position.z);
        transform.name = tileCoord.x.ToString() + "," + tileCoord.y.ToString();

    }

    public void EntityAction(Vector3 direction)
    {
        if(currentTileEnitity != null)
            currentTileEnitity.GetComponent<Pawn>().Move(direction);
    }

    // Visually swap the material of the tile
    public void SwapMaterial(int targetMat)
    {
        Renderer rend = GetComponent<Renderer>();
        switch (targetMat)
        {
            case 0:
                rend.material = defaultMat;
                break;
            case 1:
                rend.material = highlightMat;
                break;
            default:
                Debug.Log("wut");
                break;
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
    public GameObject TopTile
    {
        get { return topTile; }
        set { topTile = value; }
    }
    public GameObject RightTile
    {
        get { return rightTile; }
        set { rightTile = value; }
    }
    public GameObject BottomTile
    {
        get { return bottomTile; }
        set { bottomTile = value; }
    }
    public GameObject LeftTile
    {
        get { return leftTile; }
        set { leftTile = value; }
    }
}
