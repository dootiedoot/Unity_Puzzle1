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
    //[HideInInspector]
    public bool isSelected = false;
    private Material defaultMat;
    public Color defaultColor;
    public Color flashColor;

    // Use this for initialization
    void Awake ()
    {
        defaultMat = GetComponent<Renderer>().material;
        tileCoord = new Vector2(transform.position.x, transform.position.z);
        transform.name = tileCoord.x.ToString() + "," + tileCoord.y.ToString();
    }

    void Start()
    {
        defaultMat.color = defaultColor;
    }

    public void EntityAction(Vector3 direction)
    {
        if(currentTileEnitity != null)
            currentTileEnitity.GetComponent<Entity>().Move(direction);
    }

    public void FlashColor(){
        isSelected = true;
        StartCoroutine(doFlashColor());
    }

    IEnumerator doFlashColor()
    {
        float flashSpeed = 2;
        float flashingTimer = 0;

        Color defaultColor = this.defaultColor;
        Color flashColor = this.flashColor;

        while(isSelected)
        {
            defaultMat.color = Color.Lerp(defaultColor, flashColor, Mathf.PingPong(flashingTimer * flashSpeed, 1));

            flashingTimer += Time.deltaTime;
            yield return null;
        }
        defaultMat.color = defaultColor;
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
