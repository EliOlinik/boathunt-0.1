using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelector : MonoBehaviour
{
    float startHeading, targetHeading;
    Vector3 dragOrigin;
    Dictionary<GameObject, Ship> contacts = new Dictionary<GameObject, Ship>();
    List<GameObject> pingIndicators = new List<GameObject>();
    public static TargetSelector current;

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
        targetHeading = Player.current.targetIndicator.eulerAngles.y;
        PreheatPingIndicators();
    }

    private void PreheatPingIndicators()
    {
        for (int i = 0; i < Player.current.pingContatsToDisplay; ++i)
        {
            GameObject newPingIndicator = Instantiate(Player.current.pingIndicatorPrefab, Player.current.pingIndicatorParent.transform);
            pingIndicators.Add(newPingIndicator);
            newPingIndicator.SetActive(false);
        }
    }

    private void Update()
    {
        if (Player.current.currentTarget != null && !Player.current.searching)
        {            
            TrackTarget();
            return;
        }

        if (Player.current.currentTarget == null && !Player.current.searching)
        {
            Player.current.targetIndicator.Rotate(0f, Time.deltaTime * Player.current.SweepSpeed, 0f);
            SonarCheck();
            return;
        }        
    }
    private static void TrackTarget()
    {
        Player.current.targetIndicator.LookAt(Player.current.currentTarget.transform.position);
        if (Player.current.currentTarget.sinking)
        {
            Player.current.currentTarget = null;
        }
        else if (RangeIndicatorController.current.GetRange(Player.current.currentTarget._transform.position) > Player.current.maxDetectionRange)
        {
            Player.current.currentTarget = null;
        }
    }
    public void SetDragOrigin(Vector3 origin, float heading)
    {
        dragOrigin = origin;
        startHeading = heading;
    }
    public void SearchIndicatorControl()
    {
        if (!Player.current.searching)
        {
            startHeading = targetHeading;
            Player.current.searching = true;
        }

        #region Check for out of screen click
        bool outOfScreen = false;
        if (Input.mousePosition.x > Screen.width || Input.mousePosition.y > Screen.height || Input.mousePosition.x < 0f || Input.mousePosition.y < 0)
        {
            outOfScreen = true;
        }
        #endregion

        if (!outOfScreen)
        {
            SetSearchHeading();
            SonarCheck();
        }
    }
    private void SonarCheck()
    {
        Player.current.currentTarget = null;
        RaycastHit hit;
        
        if (Physics.Raycast(Player.current.targetIndicator.position, Player.current.targetIndicator.forward, out hit))
        {
            Ship contact;
            GameObject contactGO = hit.collider.gameObject;
            if (hit.collider.isTrigger && hit.collider.tag == "Ship")
            {
                if (!contacts.ContainsKey(hit.collider.gameObject))
                {
                    Ship newContactController = hit.collider.gameObject.GetComponent<Ship>();
                    contacts.Add(contactGO, newContactController);
                    contact = newContactController;
                }
                else
                {
                    contact = contacts[contactGO];
                }                
                
                if((RangeIndicatorController.current.GetRange(contact._transform.position) < Player.current.maxDetectionRange) && !contact.sinking && !contact.firedUpon)
                {
                    Player.current.currentTarget = contact;
                }
            }
        }
    }
    private void SetSearchHeading()
    {
        Vector3 dragOffset = Input.mousePosition - dragOrigin;

        float dragValueXLR = Mathf.InverseLerp(0, Screen.width, dragOffset.x);
        float dragValueXRL = Mathf.InverseLerp(0, Screen.width, -dragOffset.x);

        if (dragValueXLR > 0)
        {
            targetHeading = startHeading - dragValueXLR * 360f;
        }
        if (dragValueXRL > 0)
        {
            targetHeading = startHeading + dragValueXRL * 360f;
        }
        Player.current.targetIndicator.rotation = Quaternion.Euler(0f, targetHeading, 0f);
    }
    public void Ping()
    {

        List<Ship> sortedList = GetShipsByDistance();
        ShowPingedContacts(sortedList);
        Invoke("EndPing", Player.current.pingDuration);
    }
    private void ShowPingedContacts(List<Ship> sortedList)
    {
        for (int i = 0; i < Player.current.pingContatsToDisplay; ++i)
        {
            Vector3 dir = sortedList[i]._transform.position - Player.current._transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = lookRotation.eulerAngles;
            pingIndicators[i].transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            pingIndicators[i].SetActive(true);
        }
    }
    void EndPing()
    {
        foreach(GameObject pingIndicator in pingIndicators)
        {
            pingIndicator.SetActive(false);
        }
    }
    private List<Ship> GetShipsByDistance()
    {
        List<Ship> sortedList = new List<Ship>();
        List<Ship> unsortedList = GetActiveShipsList();
        while (unsortedList.Count > 0)
        {
            Ship closesetUnsortedShip = unsortedList[0];
            float closesetDistance = RangeIndicatorController.current.GetRange(closesetUnsortedShip._transform.position);
            foreach (Ship shipToConpare in unsortedList)
            {
                if (closesetUnsortedShip != shipToConpare)
                {
                    float distanceToCompare = RangeIndicatorController.current.GetRange(shipToConpare._transform.position);
                    if (distanceToCompare < closesetDistance)
                    {
                        closesetDistance = distanceToCompare;
                        closesetUnsortedShip = shipToConpare;
                    }
                }
            }
            unsortedList.Remove(closesetUnsortedShip);
            sortedList.Add(closesetUnsortedShip);
        }

        return sortedList;
    }
    private List<Ship> GetActiveShipsList()
    {
        List<Ship> shipDistances = new List<Ship>();
        foreach (Ship.Type shipType in LevelManager.current.activeShips.Keys)
        {
            foreach (Ship contact in LevelManager.current.activeShips[shipType])
            {
                shipDistances.Add(contact);
            }
        }
        return shipDistances;
    }
}
