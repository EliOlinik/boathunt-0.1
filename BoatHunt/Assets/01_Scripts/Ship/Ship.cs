using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour
{
    public enum Type { target, hunter, minelayer}
    public Type type;
    public float sonarDetectionRadius;
    public float tonnage;
    public float sinkingRoll, sinkingPitch;
    [HideInInspector]public string home;
    [HideInInspector] public bool sinking, firedUpon;
    Rigidbody rb;
    CapsuleCollider detectionRadiusTrigger;

    [HideInInspector] public Transform _transform;


    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(_transform.position, Player.current._transform.position);
        if (distanceToPlayer >= LevelManager.current.shipRespwanDistance && !sinking)
        {
            LevelManager.current.SpawnShip(this);
            return;
        }
    }

    public void Setup(string name)
    {
        home = name;
        sinking = false;
        firedUpon = false;
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        if (detectionRadiusTrigger == null)
        {
            detectionRadiusTrigger = gameObject.AddComponent<CapsuleCollider>();
            detectionRadiusTrigger.radius = sonarDetectionRadius;
            detectionRadiusTrigger.height = 1f;
            detectionRadiusTrigger.center = new Vector3(0f, -LevelManager.current.distanceToSurface, 0f);
            detectionRadiusTrigger.isTrigger = true;
        }
        _transform = transform;
    }

    public void Despawn()
    {
        gameObject.SetActive(false);
        ItemSpawner.current.ReturnShip(home, this);
    }    

    public void Sink()
    {
        sinking = true;        
        rb.isKinematic = false;
        rb.AddRelativeTorque(sinkingPitch, 0f, sinkingRoll);
        LevelManager.current.ReportSinking(this);
    }
}
