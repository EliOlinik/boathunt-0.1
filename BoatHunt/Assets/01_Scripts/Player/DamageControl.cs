using UnityEngine;
using System.Collections.Generic;

public class DamageControl : MonoBehaviour
{
    public static DamageControl current;
    Animator circleAnimator;
    List<Mine> nearbyMines = new List<Mine>();
    public bool canTakeDamage;
    public float currentHP;   
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
        if(circleAnimator == null)
        {
            circleAnimator = Player.current.mineWarningCircle.GetComponent<Animator>();
        }
        Player.current.mineWarningCircle.SetActive(false);
        #endregion
        Player.current.hitIndicator.SetActive(true);
        Invoke("EndCooldown", Player.current.cooldownAfterHit);
        currentHP = Player.current.maxHP;
        Invoke("UpdateUI", 0.1f);
    }

    private void UpdateUI()
    {
        UIController.current.UpdateHullIntegrety();
    }

    public void MineInWarningArea(Mine mine)
    {
        if(nearbyMines.Count == 0)
        {
            Player.current.mineWarningCircle.SetActive(true);
            circleAnimator.StartPlayback();
        }
        nearbyMines.Add(mine);
        circleAnimator.speed = nearbyMines.Count;
    }
    public void MineOutWarningArea(Mine mine)
    {
        nearbyMines.Remove(mine);
        circleAnimator.speed = nearbyMines.Count;
        if (nearbyMines.Count == 0)
        {
            circleAnimator.StopPlayback();
            Player.current.mineWarningCircle.SetActive(false);
        }
    }
    public void HitMine(float damage)
    {
        if (canTakeDamage)
        {
            float damageVariation = (LevelManager.current.mineDamageVariation / 100f) * damage;
            currentHP -= damage + Random.Range(-damageVariation, damageVariation);
            if (currentHP <= 0)
            {
                currentHP = 0;
                GameManager.current.GameOver(true);
            }
            canTakeDamage = false;
            Player.current.hitIndicator.SetActive(true);
            Invoke("EndCooldown", Player.current.cooldownAfterHit);
            UpdateUI();
        }
    }

    public void EndCooldown()
    {
        Player.current.hitIndicator.SetActive(false);
        canTakeDamage = true;
    }
}
