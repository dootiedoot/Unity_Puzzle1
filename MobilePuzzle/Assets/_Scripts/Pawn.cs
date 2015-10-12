using UnityEngine;
using System.Collections;

public class Pawn : MonoBehaviour
{
    [SerializeField] private Vector2 currentCoords;
    [SerializeField] private GameObject currentTile;

    public float duration;
    public int distance;

    private bool isMoving = false;

    //private GameObject[] tiles;

    //enum Direction { Up, Down, Left, Right };
    //Direction myDirection;

    void Start()
    {
        /*
        currentCoords = new Vector2(transform.position.x, transform.position.z);
        string tileName = transform.position.x + "," + transform.position.z;
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach(GameObject tile in tiles)
        {
            if(tile.name == tileName)
            {
                currentTile = tile;
                tile.GetComponent<Tile>().CurrentTileEnitity = gameObject;
            }
        }*/
    }
	
	// Update is called once per frame
	void Update ()
    {

	}


    public void Move(Vector3 direction)
    {
        if (!isMoving)
            StartCoroutine(Move(transform, direction, distance, duration));
    }

    IEnumerator Move(Transform source, Vector3 direction, int distance, float overTime)
    {
        isMoving = true;
        Vector3 newPosition = source.position + direction * distance;
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            source.position = Vector3.Lerp(source.position, newPosition, (Time.time - startTime) / overTime);
            yield return null;
        }
        source.position = newPosition;
        isMoving = false;
    }

    // Accessors and Mutators
    public GameObject CurrentTile
    {
        get { return currentTile; }
        set { currentTile = value; }
    }
}
