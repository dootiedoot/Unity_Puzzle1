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
    //[SerializeField] private GameObject currentTile;
    private Tile currentTile;
    private Tile previousTile;

    // Moving Attributes
    private float moveSpeed;
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
        foreach (Tile _tile in MapGenerator.tiles)
        {
            if (_tile.TileCoord == currentCoords)
            {
                //currentTile = tile;
                currentTile = _tile;
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
        foreach (Tile _tile in MapGenerator.tiles)
        {
            if (_tile.TileCoord == currentCoords)
            {
                //currentTile = tile;
                currentTile = _tile;
                if (!_tile.TileEnitities.Contains(gameObject))
                    _tile.TileEnitities.Add(gameObject);
                if (previousTile.TileCoord != currentTile.TileCoord)
                    previousTile.RemoveEntity(gameObject);
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
    public int GetMoveAmount(Vector3 direction, Tile currentTile)
    {
        if (moveAmount < moveDistance)
        {
            Tile _tile = currentTile;
            previousDirection = direction;
            if (direction == Vector3.forward && _tile.TopTile && isWalkableTile(_tile.TopTile))
            {
                moveAmount++;
                GetMoveAmount(direction, _tile.TopTile);
            }
            else if (direction == Vector3.right && _tile.RightTile && isWalkableTile(_tile.RightTile))
            {
                moveAmount++;
                GetMoveAmount(direction, _tile.RightTile);
            }
            else if (direction == Vector3.back && _tile.BottomTile && isWalkableTile(_tile.BottomTile))
            {
                moveAmount++;
                GetMoveAmount(direction, _tile.BottomTile);
            }
            else if (direction == Vector3.left && _tile.LeftTile && isWalkableTile(_tile.LeftTile))
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
            // if target ahead is an a interactive and pusher is a player then move 1 unit
            if (tile.ContainsEntityTag(Tags.Interactive) && CompareTag(Tags.Player))
            {
                if (tile.GetEntityByTag(Tags.Interactive).GetComponent<EntityMotor>().GetMoveAmount(previousDirection, tile) != 0)
                {
                    tile.GetEntityByTag(Tags.Interactive).GetComponent<EntityMotor>().Move(previousDirection);
                    moveAmount++;
                }
                return isWalkable = false;
            }
            // if target ahead is a destructor & pusher is a player then move 1 unit
            if (tile.ContainsEntityTag(Tags.Destructor) && CompareTag(Tags.Player))
            {
                EntityMotor _entityMotor = tile.GetEntityByTag(Tags.Destructor).GetComponent<EntityMotor>();
                if (_entityMotor.GetMoveAmount(previousDirection, tile) != 0)
                {
                    _entityMotor.Move(previousDirection);
                    moveAmount++;
                }
                return isWalkable = false;
            }
            // if target ahead is an a Lock & pusher is a interactive then do a lock type check and attempt to unlock
            else if (tile.ContainsEntityTag(Tags.Interactable) && CompareTag(Tags.Interactive))
            {
                EntityInteractive _entityInteractive = GetComponent<EntityInteractive>();
                Interactable _interactable = tile.GetEntityByTag(Tags.Interactable).GetComponent<Interactable>();
                if ((int)_entityInteractive.myType == (int)_interactable.acceptedType)
                {
                    moveAmount++;
                    return isWalkable = false;
                }
            }
            // if target ahead is an exit & pusher is a player then do a exit type check and move
            else if (tile.ContainsEntityTag(Tags.Exit) && CompareTag(Tags.Player))
            {
                if ((int)tile.GetEntityByTag(Tags.Exit).GetComponent<Exit>().acceptedType == (int)_EntityType.myType)
                {
                    moveAmount++;
                    return isWalkable = false;
                }
            }
            // if the target ahead is anything else then don't move (obstacle, player, etc.)
            else
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
            moveAmount = 0;
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
        
        isMoving = false;
        GameManager.IsPlayerMoving = false;

        // Send an event after players have finished moving
        if (OnAction != null)
            OnAction();

    }

    // Accessors and Mutators
    public Tile CurrentTile
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
