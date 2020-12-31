using UnityEngine;
using System.Collections;
using Pixeye.Unity;


public class Player : MonoBehaviour
{
    public static Player current;

    #region Movement settings
    [Foldout("Movement settings", true)]
    public ParticleSystem bubbleTrail;
    public Transform turnTarget;
    public float turnRate;
    public float maxSpeed, acceleration, decceleration;
    public float mineWarningDistance;
    #endregion
    #region HP settings
    [Foldout("HP settings", true)]
    public float maxHP;
    public float cooldownAfterHit;
    public float cooldownFlashingRate;
    public GameObject hitIndicator;
    #endregion
    #region Torpedo and Fire control settings
    [Foldout("Fire Control Settings", true)]
    public Ship currentTarget;
    public float fireRange;

    [Foldout("Torpedo Preheat settings", true)]
    public GameObject torpedoPrefab;
    public Transform torpedoStorage;
    public int torpedoesToPreheat;
    public float torpedoFireRate;

    [Foldout("Torpedo Behaviour settings",true)]
    public bool timedLifetime;
    public float torpedoSpeed;
    public float torpedoLifetime;
    public float torpedoTurnRate;
    public float torpedoHitRange;

    #endregion
    #region Indicators
    [Foldout("Indicators", true)]
    public GameObject mineWarningCircle;
    public GameObject mineWarningSign;
    public GameObject pingIndicatorParent;
    public GameObject pingIndicatorPrefab;
    public Transform headingIndincator;
    public Transform targetIndicator;
    public Transform leftBlocker;
    public Transform rightBlocker;
    #endregion
    #region Target selection settings    
    [Foldout("Target Selection Settings", true)]
    public float maxDetectionRange;
    public float minDetectionRange;
    public float pingDuration;
    public int pingContatsToDisplay;
    public float SweepSpeed;
    #endregion

    [HideInInspector] public Transform _transform;
    [HideInInspector] public bool dragging, searching;


    private void OnEnable()
    {
        #region Initialization
        if (current != this)
        {
            if (current != null)
            {
                Destroy(current);                
            }
            current = this;
        }
        #endregion
        gameObject.AddComponent<MovmentController>();
        gameObject.AddComponent<RangeIndicatorController>();
        gameObject.AddComponent<TargetSelector>();
        gameObject.AddComponent<FireControl>();
        gameObject.AddComponent<DamageControl>();     
        
        _transform = transform;
    }

    public void Launch()
    {
        FireControl.current.Launch();
    }

    public void Ping()
    {
        TargetSelector.current.Ping();
    }
}
