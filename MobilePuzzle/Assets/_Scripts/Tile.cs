using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    // VARIABLES
    // Tile
    [SerializeField] private Vector2 tileCoord;
    int xCoord;
    int yCoord;

    public List<GameObject> TileEnitities = new List<GameObject>();
    [SerializeField] private GameObject topTile;
    [SerializeField] private GameObject rightTile;
    [SerializeField] private GameObject bottomTile;
    [SerializeField] private GameObject leftTile;

    // visuals
    //[HideInInspector]
    public bool isSelected = false;
    private Material defaultMat;
    //[SerializeField]
    private Color defaultColor;
    [SerializeField] private Color flashColor;

    // Use this for initialization
    void Awake ()
    {
        tileCoord = new Vector2(transform.position.x, transform.position.z);
        transform.name = tileCoord.x.ToString() + "," + tileCoord.y.ToString();
    }

    void Start()
    {
        defaultMat = GetComponent<Renderer>().material;
        defaultColor = defaultMat.color;
    }

    public GameObject[] GetAdjacentTiles()
    {
        GameObject[] adjacentTiles = new GameObject[5];
        adjacentTiles[0] = topTile;
        adjacentTiles[1] = rightTile;
        adjacentTiles[2] = bottomTile;
        adjacentTiles[3] = leftTile;
        adjacentTiles[4] = gameObject;
        return adjacentTiles;
    }

    public void EntityAction(Vector3 direction)
    {
        if(TileEnitities.Count != 0)
        {
            foreach(GameObject tileEntity in TileEnitities)
            {
                if (tileEntity.CompareTag("Player"))
                    tileEntity.GetComponent<EntityMotor>().Move(direction);
            }
        }
    }

    public bool ContainsEntityTag(string tag)
    {
        bool doesContain = false;
        if (TileEnitities.Count != 0)
            foreach (GameObject tileEntity in TileEnitities)
                if (tileEntity != null && tileEntity.CompareTag(tag))
                    return doesContain = true;
        return doesContain;
    }

    public GameObject GetEntityByTag(string tag)
    {
        GameObject retrivalEntity = null;
        if (TileEnitities.Count != 0)
            foreach (GameObject tileEntity in TileEnitities)
                if (tileEntity.CompareTag(tag))
                    return retrivalEntity = tileEntity;
        return retrivalEntity;
    }

    public void RemoveEntity(GameObject entity)
    {
        if (TileEnitities.Count != 0)
        {
            GameObject removalEntity = null;

            foreach (GameObject tileEntity in TileEnitities)
                if (tileEntity == entity)
                    removalEntity = tileEntity;

            TileEnitities.Remove(removalEntity);
        }
    }

    public void FlashColor()
    {
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
