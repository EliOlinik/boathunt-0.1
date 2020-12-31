using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeIndicatorController : MonoBehaviour
{
    public static RangeIndicatorController current;
    static float rangeIndication;

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
    }
    public void Update()
    {
        if (Player.current.currentTarget == null)
        {
            UIController.current.UpdateDebugText("No Target");
            rangeIndication = 1f;
            SetBlockers(89f);
        }
        else if (Player.current.currentTarget != null)
        {
            string rangeToTarget = RangeIndicatorController.current.GetRange(Player.current.currentTarget._transform.position).ToString("F0");
            string type = Player.current.currentTarget.type.ToString();
            UIController.current.UpdateDebugText("Current Target: " + type + "\nRange To Target: " + rangeToTarget);
            SetRangeIndicator(Player.current.currentTarget._transform.position);
            SetBlockers(Mathf.Lerp(85f, 0f, rangeIndication));
        }
    }

    public void SetRangeIndicator(Vector3 contact)
    {        
        float range = GetRange(contact);
        rangeIndication = Mathf.InverseLerp(Player.current.maxDetectionRange, Player.current.minDetectionRange, range);
    }

    private void SetBlockers(float precentage)
    {
        Player.current.leftBlocker.localEulerAngles = new Vector3(90f, -precentage, 0f);
        Player.current.rightBlocker.localEulerAngles = new Vector3(90f, precentage, 0f);
    }

    public float GetRange(Vector3 target)
    {
        float dX = target.x - Player.current._transform.position.x;
        float dZ = target.z - Player.current._transform.position.z;
        return new Vector2(dX, dZ).magnitude;
    }
}
