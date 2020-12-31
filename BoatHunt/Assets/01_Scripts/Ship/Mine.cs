using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour
{
    Transform _transform;
    float distanceToPlayer;
    bool inWarningRange;
    public string home;
    public float damage;

    private void OnEnable()
    {
        if (_transform == null)
        {
            _transform = transform;
        }
    }

    private void Update()
    {
        distanceToPlayer = Vector3.Distance(_transform.position, Player.current._transform.position);
        if (distanceToPlayer >= LevelManager.current.maxDistanceFormPlayer)
        {
            Respawn();
            return;
        }
        if(distanceToPlayer <= Player.current.mineWarningDistance && !inWarningRange)
        {
            inWarningRange = true;
            DamageControl.current.MineInWarningArea(this);
        }
        else if(distanceToPlayer > Player.current.mineWarningDistance && inWarningRange)
        {
            inWarningRange = false;
            DamageControl.current.MineOutWarningArea(this);
        }        
    }
    public void Setup()
    {
        SpawnAround();

        distanceToPlayer = Vector3.Distance(_transform.position, Player.current._transform.position);
        while (distanceToPlayer <= LevelManager.current.spanwSafetyDistance || distanceToPlayer >= LevelManager.current.maxDistanceFormPlayer)
        {
            if (LevelManager.current.maxDistanceFormPlayer == 0 || LevelManager.current.maxDistanceFormPlayer == 0)
            {
                Debug.Log("Check mine spawn settings!");
                return;
            }
            SpawnAround();
        }
        _transform.parent = null;
        gameObject.SetActive(true);
    }


    public void Respawn()
    {
        _transform.parent = Player.current._transform;
        SpawnFront();
        
        while (distanceToPlayer <= LevelManager.current.spanwSafetyDistance && distanceToPlayer >= LevelManager.current.maxDistanceFormPlayer)
        {
            if (LevelManager.current.maxDistanceFormPlayer == 0 || LevelManager.current.maxDistanceFormPlayer == 0)
            {
                Debug.Log("Check mine spawn settings!");
                return;
            }
            SpawnFront();
        }
        _transform.parent = null;
    }

    private void OnTriggerEnter(Collider other)
    {        
        if(other.tag == "Player" && DamageControl.current.canTakeDamage)
        {
            DamageControl.current.HitMine(damage);
            SpawnFront();
        }
    }

    private void SpawnAround()
    {
        float randomInitialX = Random.Range(-LevelManager.current.maxDistanceFormPlayer, LevelManager.current.maxDistanceFormPlayer);
        float randomInitialZ = Random.Range(-LevelManager.current.maxDistanceFormPlayer, LevelManager.current.maxDistanceFormPlayer);
        _transform.position = Player.current._transform.position + new Vector3(randomInitialX, 0f, randomInitialZ);
        distanceToPlayer = Vector3.Distance(_transform.position, Player.current._transform.position);
    }
    private void SpawnFront()
    {
        float randomInitialX = Random.Range(-LevelManager.current.maxDistanceFormPlayer, LevelManager.current.maxDistanceFormPlayer);
        float randomInitialZ = Random.Range(LevelManager.current.spanwSafetyDistance, LevelManager.current.maxDistanceFormPlayer);
        _transform.localPosition = new Vector3(randomInitialX, 0f, randomInitialZ);
    }

    public void Despawn()
    {
        LevelManager.current.activeMines.Remove(this);
        ItemSpawner.current.ReturnMine(home, this);
        gameObject.SetActive(false);
    }
}
