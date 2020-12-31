using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FireControl : MonoBehaviour
{
    public static FireControl current;
    List<Torpedo> torpedoPool = new List<Torpedo>();
    bool canFire;

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
        Preheat(Player.current.torpedoesToPreheat);
        canFire = true;
    }
    private void Update()
    {
        Ship target = Player.current.currentTarget;
        if(target != null)
        {
            Vector3 targetPosition = Player.current.currentTarget._transform.position;
            float currentRange = RangeIndicatorController.current.GetRange(targetPosition);
            float fireRange = Player.current.fireRange;            
            if (currentRange <= fireRange && canFire)
            {
                canFire = false;
                StartCoroutine(Launch());
            }
        }
    }

    void Preheat(int amount)
    {
        for(int i = 0; i < amount; ++i)
        {
            Torpedo newTorpedo = Instantiate(Player.current.torpedoPrefab, Player.current.torpedoStorage).GetComponent<Torpedo>();
            newTorpedo.gameObject.name = Player.current.torpedoPrefab.name + "_" + i;
            newTorpedo.gameObject.SetActive(false);
            torpedoPool.Add(newTorpedo);
        }
    }

    public IEnumerator Launch()
    {

        if(torpedoPool.Count == 0)
        {
            Preheat(1);
        }

        torpedoPool[0].Launch(Player.current.currentTarget);
        torpedoPool[0]._transform.parent = null;
        torpedoPool.RemoveAt(0);
        Player.current.currentTarget.firedUpon = true;
        Player.current.currentTarget = null;
        yield return new WaitForSeconds(Player.current.torpedoFireRate + 0.001f);

        canFire = true;
    }

    public void Return(Torpedo torpedoToReturn)
    {
        torpedoPool.Add(torpedoToReturn);
    }
}
