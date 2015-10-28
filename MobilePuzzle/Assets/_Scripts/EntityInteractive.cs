using UnityEngine;
using System.Collections;

public class EntityInteractive : MonoBehaviour
{
    [SerializeField]
    private bool isMovable;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    // Accessors and Mutators
    public bool IsMoveable
    {
        get { return isMovable; }
        set { isMovable = value; }
    }
}
