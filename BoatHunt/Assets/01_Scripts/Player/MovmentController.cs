using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovmentController : MonoBehaviour
{
    Vector3 dragOrigin;
    Transform _transform;
    float markerRotation, markerStartRotation, currentSpeed;
    ParticleSystem.MainModule PPmain;
    ParticleSystem.EmissionModule PPemission;
    public static MovmentController current;       
    
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
        _transform = transform;
        markerRotation = Player.current.headingIndincator.eulerAngles.y;
        PPmain = Player.current.bubbleTrail.main;
        PPemission = Player.current.bubbleTrail.emission;
    }

    private void Update()
    {
        if(GameManager.current.pause)
        {
            return;
        }
        Player.current.dragging = Input.GetMouseButton(0);
        if(!Player.current.searching)
        {
            HeadingIndicatorControl();
        }
        ShipMovmentControl();
        PPemission.rateOverTime = Mathf.Lerp(0, 300, currentSpeed / Player.current.maxSpeed);
    }

    private void ShipMovmentControl()
    {
        SetSpeed();
        SetHeading();
        _transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }

    private void SetHeading()
    {        
        if(currentSpeed == 0)
        {
            return;
        }
        Vector3 dir = Player.current.turnTarget.position - _transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir); //flip to "-dir" for world-space canvas!
        Vector3 rotation = Quaternion.Lerp(_transform.rotation, lookRotation, (Time.deltaTime * Player.current.turnRate) * (currentSpeed/Player.current.maxSpeed)).eulerAngles;

        _transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);       
    }

    private void SetSpeed()
    {
        if (Input.GetMouseButton(0) && !Player.current.searching)
        {
            if (currentSpeed < Player.current.maxSpeed)
            {
                currentSpeed += Player.current.acceleration * Time.deltaTime;
            }
            else
            {
                currentSpeed = Player.current.maxSpeed;
            }
        }
        else
        {
            if (currentSpeed > 0)
            {
                currentSpeed -= Player.current.decceleration * Time.deltaTime;
            }
            else
            {
                currentSpeed = 0f;
            }
        }
    }

    private void HeadingIndicatorControl()
    {

        if (Input.GetMouseButtonDown(0))
        {
            markerStartRotation = markerRotation;            
            dragOrigin = Input.mousePosition;
        }
        if (Player.current.dragging)
        {
            #region Check for out of screen click
            bool outOfScreen = false;
            if (Input.mousePosition.x > Screen.width || Input.mousePosition.y > Screen.height || Input.mousePosition.x < 0f || Input.mousePosition.y < 0)
            {
                outOfScreen = true;
            }
            #endregion
            
            if (!outOfScreen)
            {             
                float mouseOffset = GetMouseOffset();                                                                              
                markerRotation = markerStartRotation + mouseOffset;
                if (markerRotation >= 180)
                {
                    markerRotation = (-180 + (markerRotation - 180));
                }
                else if (markerRotation <= -180)
                {
                    markerRotation = (180 + (markerRotation + 180));
                }
                Player.current.headingIndincator.rotation = Quaternion.Euler(0f, markerRotation, 0f);
            }
        }
    }  

    public float GetMouseOffset()
    {
        Vector3 dragOffset = Input.mousePosition - dragOrigin;
        float dragValueXLR = Mathf.InverseLerp(0, Screen.width, dragOffset.x);
        float dragValueXRL = Mathf.InverseLerp(0, Screen.width, -dragOffset.x);
        
        float mouseOffset=0;
        if (dragValueXLR > 0)
        {
            mouseOffset = -360 * dragValueXLR;
            if (mouseOffset <= -360)
            {
                mouseOffset += 360;
            }
        }
        if (dragValueXRL > 0)
        {
            mouseOffset = 360 * dragValueXRL;                   
            if (mouseOffset >= 360)
            {
                mouseOffset -= 360;
            }
        }

        return mouseOffset;
    }
}
