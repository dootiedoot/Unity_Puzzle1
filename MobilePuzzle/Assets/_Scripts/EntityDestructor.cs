using UnityEngine;
using System.Collections;

public class EntityDestructor : MonoBehaviour
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

        Tile[] adjacentTiles = _entityMotor.CurrentTile.GetAdjacentTiles();
        for (int i = 0; i < 4; i++)
        {
            if (adjacentTiles[i] != null)
            {
                Tile _tile = adjacentTiles[i].GetComponent<Tile>();
                if (_tile.ContainsEntityTag(Tags.Destructible))
                {
                    _tile.GetEntityByTag(Tags.Destructible).GetComponent<Destructible>().Detonate();
                }
            }
        }
        _entityMotor.CurrentTile.RemoveEntity(gameObject);
        Destroy(gameObject);
    }
}
