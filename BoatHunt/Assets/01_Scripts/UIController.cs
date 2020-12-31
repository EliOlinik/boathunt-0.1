using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public TMP_Text shipAlive;
    public TMP_Text scoreCounter;
    public TMP_Text hullIntegrety;
    public TMP_Text debugText;
    
    
    public static UIController current;
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
    public void UpdateShipCounter()
    {
        shipAlive.text = "Cargo: " + LevelManager.current.activeShips[Ship.Type.target].Count + "/" + LevelManager.current.targetsToSpawn + 
                       "\nMinelayers: " + LevelManager.current.activeShips[Ship.Type.minelayer].Count + "/" + LevelManager.current.minelayersToSpawn + 
                       "\nHunters: " + LevelManager.current.activeShips[Ship.Type.hunter].Count + "/" + LevelManager.current.huntersToSpawn;
    }
    public void UpdateScoreCounter()
    {
        scoreCounter.text = "Tonnage Sunk: \n" + GameManager.current.tonnageSunk; 
    }
    public void UpdateHullIntegrety()
    {
        hullIntegrety.text = "Hull Integrety:" + "\n" + ((DamageControl.current.currentHP / Player.current.maxHP)*100f).ToString("F0") + "%";
    }
    public void UpdateDebugText(string text)
    {
        debugText.text = text;
    }

    public void SetDisplayState(bool state)
    {
        shipAlive.gameObject.SetActive(state);
        scoreCounter.gameObject.SetActive(state);        
        debugText.gameObject.SetActive(state);
    }
}
