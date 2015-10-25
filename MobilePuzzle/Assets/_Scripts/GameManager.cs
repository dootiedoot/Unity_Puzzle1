using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    GameObject[] players;
    public static bool isPlayerTurn = true;
    public static int playerCount = 0;
    public static bool isEnemyTurn;

    // Use this for initialization
    void Start ()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
