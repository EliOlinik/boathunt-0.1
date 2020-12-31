using UnityEngine;
using System.Collections.Generic;
public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner current;              
    public Item[] items;       
    public Dictionary<string, List<GameObject>> itemsPool = new Dictionary<string, List<GameObject>>();
    public Dictionary<string, Transform> poolParents = new Dictionary<string, Transform>();    

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
        PreHeat();
    }    
    private void PreHeat()
    {
        foreach (Item item in items)
        {
            bool inPool = itemsPool.ContainsKey(item.prefab.name);
            if (!inPool)
            {
                Transform newParent = new GameObject().transform;
                newParent.transform.parent = this.transform;
                newParent.name = item.prefab.name;

                poolParents.Add(item.prefab.name, newParent.transform);
                itemsPool.Add(item.prefab.name, PopulatePool(item.prefab, item.initialPoolSize));
            }
            else
            {
                Debug.Log("Item in Pool");
            }
        }
    }

    public List<GameObject> PopulatePool(GameObject _prefab, int amount)
    {
        List<GameObject> newPool = new List<GameObject>();
        for (int i = 0; i < amount; ++i)
        {
            GameObject newItem = Instantiate(_prefab);
            newItem.transform.parent = poolParents[_prefab.name].transform;
            newItem.SetActive(false);
            newPool.Add(newItem);
        }
        return newPool;
    }
    
    public List<Item> GetPoolsForTpye(Item.Type typeToSpawn)
    {
        List<Item> compatibleItems = new List<Item>();

        foreach (Item currentItem in items)
        {
            if (currentItem.spawn && currentItem.type == typeToSpawn)
            {
                compatibleItems.Add(currentItem);
            }
        }

        return compatibleItems;
    }

    public List<Ship> GetShipTypeList(Ship.Type shipTypeToSpawn, int amount)
    {
        List<Item> compatibleItems = GetPoolsForTpye(Item.Type.ship);
        List<Ship> spawnedShipsList = new List<Ship>();

        while (spawnedShipsList.Count < amount)
        {
            int randomCompatibaleItem = Random.Range(0, compatibleItems.Count);
            string itemName = compatibleItems[randomCompatibaleItem].prefab.name;
            List<GameObject> corespondingPool = itemsPool[itemName];
            float spawnChance = Random.Range(0f, 100f);
            
            #region Empty Pool check
            if (corespondingPool.Count == 0)
                    {
                        corespondingPool.AddRange(PopulatePool(compatibleItems[randomCompatibaleItem].prefab, 5));
                    }
            #endregion            

            if(compatibleItems[randomCompatibaleItem].spawnProb > spawnChance)
            {                
                Ship spanwedShip = corespondingPool[0].GetComponent<Ship>();
                if(spanwedShip.type == shipTypeToSpawn)
                {
                    spanwedShip.transform.parent = null;
                    spanwedShip.Setup(itemName);
                    spawnedShipsList.Add(spanwedShip);
                    corespondingPool.RemoveAt(0);                
                }
            }
        }
        return spawnedShipsList;
    }
    public List<Mine> GetMineList(int amount)
    {
        List<Item> mineItemsList = GetPoolsForTpye(Item.Type.mine);
        List<Mine> spawnedMinesList = new List<Mine>();
        while (spawnedMinesList.Count < amount)
        {
            int randomCompatibaleItem = Random.Range(0, mineItemsList.Count);
            string itemName = mineItemsList[randomCompatibaleItem].prefab.name;
            List<GameObject> corespondingPool = itemsPool[itemName];
            float spawnChance = Random.Range(0f, 100f);

            #region Empty Pool check
            if (corespondingPool.Count == 0)
            {
                corespondingPool.AddRange(PopulatePool(mineItemsList[randomCompatibaleItem].prefab, 5));
            }
            #endregion            

            if (mineItemsList[randomCompatibaleItem].spawnProb > spawnChance)
            {
                Mine spawnedMine = corespondingPool[0].GetComponent<Mine>();
                spawnedMine.home = itemName;
                spawnedMinesList.Add(spawnedMine);
                corespondingPool.RemoveAt(0);
            }
        }
        return spawnedMinesList;
    }
    public void ReturnShip(string name, Ship ship)
    {
        if(LevelManager.current.activeShips[ship.type].Contains(ship))
        {
            LevelManager.current.activeShips[ship.type].Remove(ship);
        }
        else if(LevelManager.current.sinkingShips.Contains(ship))
        {
            LevelManager.current.sinkingShips.Remove(ship);
        }
        else
        {
            Debug.Log("Lost Ship...");
        }
        ship.transform.parent = poolParents[name];
        itemsPool[name].Add(ship.gameObject);
    }

    public void ReturnMine(string name, Mine mine)
    {
        mine.transform.parent = poolParents[name];
        itemsPool[name].Add(mine.gameObject);
    }
}
