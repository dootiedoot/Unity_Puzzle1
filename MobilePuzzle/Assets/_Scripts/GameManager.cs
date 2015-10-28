using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] players;
    public static bool isPlayerTurn = true;
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
        if(isPlayerTurn)
            Debug.Log("Players Turn");
	}

    void CheckTurn()
    {
        foreach(GameObject player in players)
        {
            EntityMotor _entityMotor = player.GetComponent<EntityMotor>();
            if (_entityMotor.IsMoving)
            {
                Debug.Log("not ur turn!");
                isPlayerTurn = false;
                break;
            }
            else
                isPlayerTurn = true;
            
        }
    }
}
