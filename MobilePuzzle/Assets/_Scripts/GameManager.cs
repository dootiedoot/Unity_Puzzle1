using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] players;
    public static bool IsPlayerMoving;
    public static bool IsPlayerTurn = true;
    [SerializeField]
    private bool isPlayerMoving;
    [SerializeField]
    private bool isPlayerTurn;
    public static int playerCount = 0;
    public static bool isEnemyTurn;

    void OnEnable()
    {
        EntityMotor.OnAction += CheckTurn;
    }

    void OnDisable()
    {
        EntityMotor.OnAction -= CheckTurn;
    }

    // Use this for initialization
    void Start ()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }
	
	// Update is called once per frame
	void Update ()
    {
        isPlayerMoving = IsPlayerMoving;
        isPlayerTurn = IsPlayerTurn;
	}

    void CheckTurn()
    {

    }
}
