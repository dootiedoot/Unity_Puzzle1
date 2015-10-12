using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{
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
                    print(hit.collider.GetComponent<Tile>().TileCoord);
                }
            }
        }
    }

    public GameObject[] GetMoveTiles()
    {
     //       1,0
     //  0,1  1,1  2,1
     //       1,2
        return null;
    }

}
