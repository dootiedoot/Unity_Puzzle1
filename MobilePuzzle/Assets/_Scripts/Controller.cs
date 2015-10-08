﻿using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{
    public GameObject obj;
    public float duration;
    public int distance;

    private bool isMoving= false;

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
                    string[] coords = hit.collider.name.Split(',');
                    int xCoord = int.Parse(coords[0]);
                    int yCoord = int.Parse(coords[1]);
                    print(xCoord + "," + yCoord);
                }
            }
        }
    }

    public void Move(Vector3 direction){
        if(!isMoving)
            StartCoroutine(Move(obj.transform, direction, distance, duration));
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
}
