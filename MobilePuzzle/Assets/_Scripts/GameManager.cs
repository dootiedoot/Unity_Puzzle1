using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] players;
    public static bool IsPlayerMoving;
    public static bool IsPlayerTurn;
    [SerializeField]
    private bool isPlayerMoving;
    [SerializeField]
    private bool isPlayerTurn;
    public static int PlayerCount;
    public static bool isEnemyTurn;

    // UI
    public Text moveCounterTextObj;
    public static Text moveCounterText;

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
        players = GameObject.FindGameObjectsWithTag(Tags.Player);
        PlayerCount = 0;
        IsPlayerMoving = false;
        IsPlayerTurn = true;
        moveCounterText = moveCounterTextObj;
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

    public static void AdjustPlayerMoveCount(int amount)
    {
        PlayerCount += amount;
        moveCounterText.text = "Moves: " + PlayerCount;
    }
}
