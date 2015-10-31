using UnityEngine;
using System.Collections;

public class Destructor : MonoBehaviour
{

    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private float DetonationDelay;

    private EntityMotor _entityMotor;

    void Awake()
    {
        _entityMotor = GetComponent<EntityMotor>();
    }

    public void Detonate()
    {
        Destroy(Instantiate(explosionPrefab, transform.position, Quaternion.identity), .5f);

        StartCoroutine(DetonateNearby());
    }

    IEnumerator DetonateNearby()
    {
        yield return new WaitForSeconds(DetonationDelay);

        GameObject[] adjacentTiles = _entityMotor._currentTile.GetAdjacentTiles();
        for (int i = 0; i < 4; i++)
        {
            if (adjacentTiles[i] != null)
            {
                Tile _tile = adjacentTiles[i].GetComponent<Tile>();
                if (_tile.ContainsEntityTag("Destructible"))
                {
                    _tile.GetEntityByTag("Destructible").GetComponent<Destructible>().Detonate();
                }
            }
        }
        _entityMotor._currentTile.RemoveEntity(gameObject);
        Destroy(gameObject);
    }
}
