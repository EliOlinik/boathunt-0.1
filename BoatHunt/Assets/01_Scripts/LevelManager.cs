using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager current;
    Transform player;
    [Header("Map Settings:")]
    public float mapWidth;
    public float mapLenght;    
    [Header("Enviroment Settings")]
    public Transform seafloor;
    public float distanceToSurface, distanceToBottom;
    [Header("Mine Settings")]
    public int minesToSpawn;
    [Range(0f,100f)]public float mineDamageVariation;
    public float spanwSafetyDistance;
    public float maxDistanceFormPlayer;
    [Header("Ship Settings")]
    public float shipRespwanDistance;    
    public int huntersToSpawn, targetsToSpawn, minelayersToSpawn;
    
    [HideInInspector] public Dictionary<Ship.Type, List<Ship>> activeShips = new Dictionary<Ship.Type, List<Ship>>();
    [HideInInspector] public List<Ship> sinkingShips = new List<Ship>();
    [HideInInspector] public List<Mine> activeMines = new List<Mine>();
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
        seafloor.position = new Vector3(0f, -distanceToBottom, 0f);
        
        Invoke("StartLevel", 0.01f);
        
    }

    private void StartLevel()
    {
        SpawnShips();
        SpawnMines();        
    }

    public void SpawnShips()
    {
        SpawnMinelayers(minelayersToSpawn);
        SpawnHunters(huntersToSpawn);
        SpawnTargets(targetsToSpawn);
        UpdateUI();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.current.TogglePause();
        }
    }    
    
    public void ResetLevel()
    {        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void SpawnMines()
    {
        DespawnMines();
        List<Mine> mines = ItemSpawner.current.GetMineList(minesToSpawn);
        foreach(Mine Mine in mines)
        {
            Mine.Setup();
            activeMines.Add(Mine);
        }
    }
    public void DespawnMines()
    {
        Debug.Log(activeMines.Count);
        while(activeMines.Count > 0)
        {
            activeMines[0].Despawn();
        }
    }

    public void SpawnMinelayers(int amount)
    {        
        List<Ship> ShipsToSpawn = ItemSpawner.current.GetShipTypeList(Ship.Type.minelayer, amount);
        int spawnTracker = 0;
        foreach(Ship ship in ShipsToSpawn)
        {
            SpawnShip(ship);
            ship.gameObject.name = ship.home + "_" + spawnTracker;
            spawnTracker++;
        }
        if(!activeShips.ContainsKey(Ship.Type.minelayer))
        {
            activeShips.Add(Ship.Type.minelayer, ShipsToSpawn);
        }
        else
        {
            activeShips[Ship.Type.minelayer].AddRange(ShipsToSpawn);
        }
    }
    public void SpawnHunters(int amount)
    {
        List<Ship> ShipsToSpawn =  ItemSpawner.current.GetShipTypeList(Ship.Type.hunter, amount);
        foreach(Ship ship in ShipsToSpawn)
        {
            SpawnShip(ship);
        }
        if (!activeShips.ContainsKey(Ship.Type.hunter))
        {
            activeShips.Add(Ship.Type.hunter, ShipsToSpawn);
        }
        else
        {
            activeShips[Ship.Type.hunter].AddRange(ShipsToSpawn);
        }
    }
    public void SpawnTargets(int amount)
    {
        List<Ship> ShipsToSpawn = ItemSpawner.current.GetShipTypeList(Ship.Type.target, amount);
        foreach (Ship ship in ShipsToSpawn)
        {
            SpawnShip(ship);

        }
        if (!activeShips.ContainsKey(Ship.Type.target))
        {
            activeShips.Add(Ship.Type.target, ShipsToSpawn);
        }
        else
        {
            activeShips[Ship.Type.target].AddRange(ShipsToSpawn);
        }
    }
    public void DespawnAll()
    {
        foreach(Ship.Type shipType in activeShips.Keys)
        {
            while(activeShips[shipType].Count > 0)
            {
                activeShips[shipType][0].Despawn();
            }
        }
        while(sinkingShips.Count > 0)
        {
            sinkingShips[0].Despawn();
        }
    }

    public void ReportSinking(Ship sinkingShip)
    {
        activeShips[sinkingShip.type].Remove(sinkingShip);
        sinkingShips.Add(sinkingShip);
        GameManager.current.tonnageSunk += sinkingShip.tonnage;
        UpdateUI();
        WinCheck();
    }

    private void WinCheck()
    {
        bool won = false;
        foreach (Ship.Type type in activeShips.Keys)
        {
            if (activeShips[type].Count == 0)
            {
                won = true;
            }
        }
        if (won)
        {
            GameManager.current.Win(true);
        }
    }

    private static void UpdateUI()
    {
        UIController.current.UpdateShipCounter();
        UIController.current.UpdateScoreCounter();
    }

    public void SpawnShip(Ship ship)
    {
        float randDist = Random.Range(spanwSafetyDistance, mapLenght);
        float randOffset = Random.Range(-mapWidth, mapWidth);
        Vector3 randShipPosition = new Vector3(randOffset, distanceToSurface, randDist);

        ship.transform.position = randShipPosition;
        ship.gameObject.SetActive(true);
    }    

    private void GameOver()
    {
        GameManager.current.GameOver(true);
    }
}
