using UnityEngine;
using System.Collections;

public class EntityMotor : MonoBehaviour
{
    // VARIABLES
    // Events
    public delegate void ClickAction();
    public static event ClickAction OnAction;

    // Tiles
    [SerializeField] private Vector2 currentCoords;
    [SerializeField] private GameObject currentTile;
    public Tile _currentTile;
    private GameObject previousTile;

    // Moving Attributes
    public float moveSpeed;
    [SerializeField] private int moveDistance;
    [SerializeField] private int moveAmount = 0;
    private Vector3 previousDirection;
    private bool isMoving = false;

    // Components
    private EntityType _EntityType;
    private Animator animator;

    void Awake()
    {
        _EntityType = GetComponent<EntityType>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        // Initial entity-to-tile setup
        currentCoords = new Vector2((int)transform.position.x, (int)transform.position.z);
        transform.position = new Vector3(currentCoords.x, transform.position.y, currentCoords.y);   // Lock transform into int coordinates
        foreach (GameObject tile in MapGenerator.tiles)
        {
            Tile _tile = tile.GetComponent<Tile>();
            if (_tile.TileCoord == currentCoords)
            {
                currentTile = tile;
                _currentTile = tile.GetComponent<Tile>();
                _tile.TileEnitities.Add(gameObject);
                previousTile = currentTile;
                break;
            }
        }
    }

    // Rescan grid to update entity-to-tile data
    void UpdatePosition()
    {
        currentCoords = new Vector2((int)transform.position.x, (int)transform.position.z);
        transform.position = new Vector3(currentCoords.x, transform.position.y, currentCoords.y);   // Lock transform into int coordinates
        foreach (GameObject tile in MapGenerator.tiles)
        {
            Tile _tile = tile.GetComponent<Tile>();
            if (_tile.TileCoord == currentCoords)
            {
                currentTile = tile;
                _currentTile = tile.GetComponent<Tile>();
                if (!_tile.TileEnitities.Contains(gameObject))
                    _tile.TileEnitities.Add(gameObject);
                if (previousTile != currentTile)
                    previousTile.GetComponent<Tile>().RemoveEntity(gameObject);
                previousTile = currentTile;
                break;
            }
        }
    }

    /*
    public void ShowMoveTiles()
    {
        CurrentTile.GetComponent<Tile>().FlashColor();
        moveAmount = 0;
        GetMoveAmount(Vector3.forward, currentTile, true);
        moveAmount = 0;
        GetMoveAmount(Vector3.right, currentTile, true);
        moveAmount = 0;
        GetMoveAmount(Vector3.back, currentTile, true);
        moveAmount = 0;
        GetMoveAmount(Vector3.left, currentTile, true);
    }*/

    // Recursion that Calculate how many tiles it can possibly move without interference.
    public int GetMoveAmount(Vector3 direction, GameObject currentTile)
    {
        if (moveAmount < moveDistance)
        {
            Tile _tile = currentTile.GetComponent<Tile>();
            previousDirection = direction;
            if (direction == Vector3.forward && _tile.TopTile && isWalkableTile(_tile.TopTile.GetComponent<Tile>()))
            {
                moveAmount++;
                GetMoveAmount(direction, _tile.TopTile);
            }
            else if (direction == Vector3.right && _tile.RightTile && isWalkableTile(_tile.RightTile.GetComponent<Tile>()))
            {
                moveAmount++;
                GetMoveAmount(direction, _tile.RightTile);
            }
            else if (direction == Vector3.back && _tile.BottomTile && isWalkableTile(_tile.BottomTile.GetComponent<Tile>()))
            {
                moveAmount++;
                GetMoveAmount(direction, _tile.BottomTile);
            }
            else if (direction == Vector3.left && _tile.LeftTile && isWalkableTile(_tile.LeftTile.GetComponent<Tile>()))
            {
                moveAmount++;
                GetMoveAmount(direction, _tile.LeftTile);
            }
        }
        moveSpeed = moveAmount;
        return moveAmount;
    }

    // If there is a entity on the tile, check for its type and return a boolean that determines if that tile is walkable
    private bool isWalkableTile(Tile tile)
    {
        bool isWalkable = true;
        if (tile.TileEnitities.Count != 0)
        {
            // if the object ahead is an a interactive and entitiy is not a interactive
            if (tile.ContainsEntityTag("Interactive") && !CompareTag("Interactive"))
            {
                if (tile.GetEntityByTag("Interactive").GetComponent<EntityMotor>().GetMoveAmount(previousDirection, tile.gameObject) != 0)
                {
                    tile.GetEntityByTag("Interactive").GetComponent<EntityMotor>().Move(previousDirection);
                    moveAmount++;
                }
                return isWalkable = false;
            }
            // if the object ahead is an a Lock and entitiy is a interactive
            else if(tile.ContainsEntityTag("Lock") && CompareTag("Interactive"))
            {
                Debug.Log("is Lock and subject is key");
                EntityInteractive _entityInteractive = GetComponent<EntityInteractive>();
                Lock _lock = tile.GetEntityByTag("Lock").GetComponent<Lock>();
                if ((int)_entityInteractive.myType == (int)_lock.acceptedType)
                {
                    Debug.Log("Key Matches");
                    moveAmount++;
                    return isWalkable = false;
                }
            }

            // if the object is a destructor & pusher is not a destructor itself then push it destructor by 1 unit
            if (tile.ContainsEntityTag("Destructor") && !CompareTag("Destructor"))
            {
                if (tile.GetEntityByTag("Destructor").GetComponent<EntityMotor>().GetMoveAmount(previousDirection, tile.gameObject) != 0)
                {
                    tile.GetEntityByTag("Destructor").GetComponent<EntityMotor>().Move(previousDirection);
                    moveAmount++;
                }
                return isWalkable = false;
            }

            else if (tile.ContainsEntityTag("Exit") && CompareTag("Player"))
            {
                if ((int)tile.GetEntityByTag("Exit").GetComponent<Exit>().acceptedType == (int)_EntityType.myType)
                {
                    moveAmount++;
                    return isWalkable = false;
                }
            }
            // if the object ahead is obstacle or another player then don't move
            else if (tile.ContainsEntityTag("Obstacle") || tile.ContainsEntityTag("Player") || tile.ContainsEntityTag("Lock") || tile.ContainsEntityTag("Destructible"))
                return isWalkable = false;
        }

        return isWalkable;
    }

    // public method to perform Move action
    public void Move(Vector3 direction)
    {
        if (!isMoving)
        {
            isMoving = true;
            GameManager.IsPlayerMoving = true;
            StartCoroutine(Move(transform, direction, GetMoveAmount(direction, currentTile), moveSpeed));
        }
    }

    IEnumerator Move(Transform source, Vector3 direction, int distance, float speed)
    {
        //transform.rotation = Quaternion.LookRotation(direction);      // To rotate object towards direction or not
        if(animator)
            animator.SetBool("isMoving", true);

        Vector3 newPosition = source.position + direction * distance;
        while (source.position != newPosition)
        {
            source.position = Vector3.MoveTowards(source.position, newPosition, Time.deltaTime * speed);
            yield return null;
        }
        source.position = newPosition;
        UpdatePosition();

        if (animator)
            animator.SetBool("isMoving", false);
        
        moveAmount = 0;
        isMoving = false;
        GameManager.IsPlayerMoving = false;

        // Send an event after players have finished moving
        if (OnAction != null)
            OnAction();

    }

    // Accessors and Mutators
    public GameObject CurrentTile
    {
        get { return currentTile; }
        set { currentTile = value; }
    }

    public int GetMoveDistance
    {
        get { return moveDistance; }
        set { moveDistance = value; }
    }
}
