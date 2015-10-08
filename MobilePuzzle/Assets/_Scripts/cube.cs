using UnityEngine;
using System.Collections;

public class cube : MonoBehaviour
{
    public GameObject Parent;
    public Vector2 currentCoords;
    enum Direction { Up, Down, Left, Right };
    Direction myDirection;

    void Start()
    {
        transform.position = new Vector3(currentCoords.x, 0, currentCoords.y);
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(myDirection == Direction.Up)
        { }
	}
}
