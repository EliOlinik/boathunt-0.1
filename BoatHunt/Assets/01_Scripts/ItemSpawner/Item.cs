using Unity;
using UnityEngine;

[System.Serializable]
public class Item
{
    public bool spawn = true;
    [Range(0f, 100f)] public float spawnProb;
    public int initialPoolSize;
    public GameObject prefab;
    public enum Type {ship, mine, depthCharge }
    public Type type;
}
