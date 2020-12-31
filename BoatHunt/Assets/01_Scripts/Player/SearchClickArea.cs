using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchClickArea : MonoBehaviour
{
    private void OnMouseDown()
    {
        if(!GameManager.current.pause)
        {
            Player.current.searching = true;
            TargetSelector.current.SetDragOrigin(Input.mousePosition, Player.current.targetIndicator.eulerAngles.y);        
        }
    }
    private void OnMouseDrag()
    {
        if(GameManager.current.pause)
        {
            Player.current.searching = false;
            return;
        }
       TargetSelector.current.SearchIndicatorControl();        
    }

    private void OnMouseUp()
    {          
       Player.current.searching = false;
    }
}
