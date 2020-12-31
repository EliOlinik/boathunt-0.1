using UnityEngine;
using System.Collections;

public class Torpedo : MonoBehaviour
{
    float lifetime;
    [SerializeField] Ship target;
    [HideInInspector] public Transform _transform;
    private void OnEnable()
    {
        lifetime = 0f;
        if (_transform == null)
        {
            _transform = transform;
        }        
    }
    private void Update()
    {
        if (target == null && target.sinking)
        {
            Despawn();
        }
        #region Setup variables
        float speed = Player.current.torpedoSpeed * Time.deltaTime;
        float turnRate = Player.current.torpedoTurnRate * Time.deltaTime;
        #endregion
        TrackTarget(speed, turnRate);
        
        float distToTarget = Vector3.Distance(_transform.position, target._transform.position);
        float hitRange = Player.current.torpedoHitRange;
        if(distToTarget < hitRange)
        {
            target.Sink();
            Despawn();
        }
        if (Player.current.timedLifetime)
        {
            LifetimeControl();
        }
    }

    private void TrackTarget(float speed, float horzTurn)
    {
        Vector3 dir = target._transform.position - _transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(_transform.rotation, lookRotation, Time.deltaTime * horzTurn).eulerAngles;

        _transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
        _transform.Translate(Vector3.forward * speed);
    }

    private void LifetimeControl()
    {
        lifetime += Time.deltaTime;
        if (lifetime > Player.current.torpedoLifetime)
        {
            Despawn();
        }
    }
    public void Launch(Ship _target)
    {
        target = _target;
        gameObject.SetActive(true);
    }
    void Despawn()
    {
        target = null;
        gameObject.SetActive(false);
        _transform.parent = Player.current.torpedoStorage;
        _transform.position = _transform.parent.position;
        _transform.rotation = _transform.parent.localRotation;
        FireControl.current.Return(this);
    }
}
