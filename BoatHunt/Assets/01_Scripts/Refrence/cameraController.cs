/*
using CustomAttributes;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public enum cameraType { drag, edgeScroll }
    [CommonSettings] public cameraType mouseFunction;
    [CommonSettings] public float panSpeed = 30f;
    [CommonSettings] public float panBorderThicknes = 10f;

    Vector3 dragOrigin, dragOffset, cameraOrigin;
    [Drag] public float dragFactor;
    

    [Header("Camera Limits:")]
    [CommonSettings] public float minY, maxY;
    [CommonSettings] public float minX, maxX;
    [CommonSettings] public float minZ, maxZ;

    [CommonSettings] public float zoomSpeed = 10f;
    [HideInInspector] public bool disableEdgeScroll = false;

    void Update()
    {
        if(GameManager.current.gameOver)
        {
            this.enabled = false;
            return;
        }
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            disableEdgeScroll = !disableEdgeScroll;
        }
        switch(mouseFunction)
        {
            case cameraType.edgeScroll:
                {
                    EdgeScrollControlls();
                    break;
                }
            case cameraType.drag:
                {
                    if(Input.GetMouseButtonDown(0))
                    {
                        cameraOrigin = transform.position;
                        dragOrigin = Input.mousePosition;
                    }
                    DragControlls(Input.GetMouseButton(0));
                    break;
                }

        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        Vector3 pos = transform.position;

        pos.y -= scroll * 1000 * zoomSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }

    private void EdgeScrollControlls()
    {
        if (Input.GetKey("w") || (Input.mousePosition.y >= Screen.height - panBorderThicknes))
        {
            if (transform.position.z >= maxZ)
            {
                return;
            }
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("s") || ((Input.mousePosition.y <= panBorderThicknes)))
        {
            if (transform.position.z <= minZ)
            {
                return;
            }
            transform.Translate(-Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("a") || ((Input.mousePosition.x <= panBorderThicknes)))
        {
            if (transform.position.x <= minX)
            {
                return;
            }
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("d") || ((Input.mousePosition.x >= Screen.width - panBorderThicknes)))
        {
            if (transform.position.x >= maxX)
            {
                return;
            }
            transform.Translate(-Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }
    }
    private void DragControlls(bool dragging)
    {
        if(dragging)
        {            
            bool outOfScreen = false;
            if(Input.mousePosition.x > Screen.width || Input.mousePosition.y > Screen.height || Input.mousePosition.x < 0f || Input.mousePosition.y < 0)
            {
                outOfScreen = true;                
                Debug.Log("Mouse Out of screen" + dragOffset);    
            }
            if (!outOfScreen)
            {
                dragOffset = Input.mousePosition - dragOrigin;

                float dragValueXLR = Mathf.InverseLerp(0, Screen.width, dragOffset.x); 
                float dragValueXRL = Mathf.InverseLerp(0, Screen.width, -dragOffset.x); 
                float dragValueYUD = Mathf.InverseLerp(0, Screen.width, -dragOffset.y); 
                float dragValueYDU = Mathf.InverseLerp(0, Screen.width, dragOffset.y);

                Vector3 cameraOffset = Vector3.zero;
                if(dragValueXLR > 0)
                {
                    cameraOffset.x = -Mathf.Lerp(0f, dragFactor, dragValueXLR);
                }
                if(dragValueXRL > 0)
                {
                    cameraOffset.x = Mathf.Lerp(0, dragFactor, dragValueXRL);
                }
                if(dragValueYDU > 0)
                {
                    cameraOffset.z = -Mathf.Lerp(0f, dragFactor, dragValueYDU);
                }
                if(dragValueYUD > 0)
                {
                    cameraOffset.z = Mathf.Lerp(0,dragFactor, dragValueYUD);
                }
                transform.position = cameraOrigin + cameraOffset;

            }            
            return;

        }
        if (Input.GetKey("w"))
        {
            if (transform.position.z >= maxZ)
            {
                return;
            }
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("s"))
        {
            if (transform.position.z <= minZ)
            {
                return;
            }
            transform.Translate(-Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("a"))
        {
            if (transform.position.x <= minX)
            {
                return;
            }
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("d"))
        {
            if (transform.position.x >= maxX)
            {
                return;
            }
            transform.Translate(-Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }
    }
}
*/
