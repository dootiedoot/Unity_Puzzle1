using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour
{
    public float Speed;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
	    transform.Rotate(0, 90 * Speed * Time.deltaTime, 0, Space.Self);
    }
}
