using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Pixeye.Unity;

public class DebugController : MonoBehaviour
{
    public static DebugController current;

    [Foldout("Debug Menu Settings", true)]
    public TMP_Dropdown settingsSelector;
    public ScrollRect settingsWindow;
    public RectTransform windowParent;
    public Toggle showAdditionalInfo;
    #region Movement Field Declerations
    [Foldout("Movment Settings", true)]
    public float maxTopSpeed, maxTurnRate, maxAcceleration, maxDeceleration;
    public GameObject topSpeed, turnRate, acceleration, deceleration;
    Slider topSpeedSlider, turnRateSlider, accelerationSlider, decelerationSlider;
    TMP_Text topSpeedText, turnRateText, accelerationText, decelerationText;
    #endregion
    #region HP Settings
    [Foldout("HP Settings", true)]
    string newCurrentHP, newCooldownTime, newMaxHP;
    public TMP_InputField currentHP, maxHP, cooldownTime;
    #endregion
    #region Fire Control Settings
    [Foldout("Fire Control Settings", true)]
    public TMP_InputField fireRate, maxDetectionRange, minDetectionRange, pingDuration, pingContacts, sweepSpeed;
    public TMP_Text fireRange;
    public Slider fireRangeSlider;
    #endregion
    #region Torpedo Settings
    [Foldout("Torpedo Settings", true)]
    public Toggle lifetime;
    public TMP_InputField torpedoLifetime, torpedoTurnRate, torpedoSpeed, torpedoHitRange;
    #endregion
    #region Level Settings
    [Foldout("LevelSettings,true")]
    public TMP_InputField mapSizeX, mapSizeZ, shipRespawnDistance, distanceToSurface, cargo, minelayes, hunters;
    #endregion
    #region Mines Settings
    public TMP_InputField maxDistance, minDistance, minesOnScene;
    public Slider dvSlide;
    public TMP_Text damageVariation;
    #endregion


    private void Start()
    {
        
        #region Movment Settings Setup
        topSpeedText = topSpeed.GetComponentInChildren<TMP_Text>();
        topSpeedSlider = topSpeed.GetComponentInChildren<Slider>();
        topSpeedSlider.value = Mathf.InverseLerp(0, maxTopSpeed, Player.current.maxSpeed);
        topSpeedText.text = "Max Speed: " + (topSpeedSlider.value * maxTopSpeed).ToString("F2");

        turnRateText = turnRate.GetComponentInChildren<TMP_Text>();
        turnRateSlider = turnRate.GetComponentInChildren<Slider>();
        turnRateSlider.value = Mathf.InverseLerp(0, maxTurnRate, Player.current.turnRate);
        turnRateText.text = "Turn Rate: " + (turnRateSlider.value * maxTurnRate).ToString("F2");

        accelerationSlider = acceleration.GetComponentInChildren<Slider>();
        accelerationText = acceleration.GetComponentInChildren<TMP_Text>();
        accelerationSlider.value = Mathf.InverseLerp(0, maxAcceleration, Player.current.acceleration);
        accelerationText.text = "Acceleration: " + (accelerationSlider.value * maxAcceleration).ToString("F2");

        decelerationSlider = deceleration.GetComponentInChildren<Slider>();
        decelerationText = deceleration.GetComponentInChildren<TMP_Text>();
        decelerationSlider.value = Mathf.InverseLerp(0, maxDeceleration, Player.current.decceleration);
        decelerationText.text = "Deceleration: " + (decelerationSlider.value * maxDeceleration).ToString("F2");
        #endregion
        #region HP setting Setup
        cooldownTime.text = Player.current.cooldownAfterHit.ToString("F2");
        currentHP.text = DamageControl.current.currentHP.ToString("F0");
        maxHP.text = Player.current.maxHP.ToString("F0");
        #endregion
        #region Target Control Setup     

        minDetectionRange.SetTextWithoutNotify(Player.current.minDetectionRange.ToString("F2"));
        maxDetectionRange.SetTextWithoutNotify(Player.current.maxDetectionRange.ToString("F2"));        
        fireRangeSlider.value = Player.current.fireRange;        
        pingDuration.text = Player.current.pingDuration.ToString();
        pingContacts.text = Player.current.pingContatsToDisplay.ToString();
        sweepSpeed.text = Player.current.SweepSpeed.ToString();
        #endregion
        #region Torpedo Setting Setup
        fireRate.text = Player.current.torpedoFireRate.ToString();
        torpedoLifetime.text = Player.current.torpedoLifetime.ToString();
        torpedoTurnRate.text = Player.current.torpedoTurnRate.ToString();
        torpedoSpeed.text = Player.current.torpedoSpeed.ToString();
        torpedoHitRange.text = Player.current.torpedoHitRange.ToString();
        #endregion 
        #region Level Setting Setup
        mapSizeX.text = LevelManager.current.mapWidth.ToString("F0");
        mapSizeZ.text = LevelManager.current.mapLenght.ToString("F0");
        distanceToSurface.text = LevelManager.current.distanceToSurface.ToString("F0");
        cargo.text = LevelManager.current.targetsToSpawn.ToString("F0");
        minelayes.text = LevelManager.current.minelayersToSpawn.ToString("F0");
        hunters.text = LevelManager.current.huntersToSpawn.ToString("F0");
        shipRespawnDistance.text = LevelManager.current.shipRespwanDistance.ToString("F0");
        #endregion
        #region Mine Settings setup
        maxDistance.text = LevelManager.current.maxDistanceFormPlayer.ToString();
        minDistance.text = LevelManager.current.spanwSafetyDistance.ToString();
        minesOnScene.text = LevelManager.current.minesToSpawn.ToString();
        damageVariation.text = LevelManager.current.maxDistanceFormPlayer.ToString();
        dvSlide.SetValueWithoutNotify(LevelManager.current.mineDamageVariation);
        damageVariation.text = "Damage amount varition: " + dvSlide.value;
        #endregion
        UpdateSettingsDisplayed();
    }


    public void WhileEditingSweepSpeed()
    {
        Player.current.SweepSpeed = float.Parse(sweepSpeed.text);
    }
    public void WhileEditingMaxRange()
    {
        Player.current.maxDetectionRange = float.Parse(maxDetectionRange.text);
    }
    public void WhileEditingMinRange()
    {        
        Player.current.minDetectionRange = float.Parse(minDetectionRange.text);
    }    
    public void WhileEditingPingDuration()
    {
        Player.current.pingDuration = float.Parse(pingDuration.text);
    }
    public void WhileEdititingPingContacts()
    {
        Player.current.pingContatsToDisplay = Mathf.RoundToInt(float.Parse(pingContacts.text));
    }
    public void OverrideFireRange()
    {
        fireRangeSlider.maxValue = Player.current.maxDetectionRange;
        fireRangeSlider.minValue = Player.current.minDetectionRange;
        fireRange.text = "Fire Range: " + Player.current.fireRange.ToString("F2");        
        Player.current.fireRange = fireRangeSlider.value;
    }    

    public void UpdateUIstate()
    {
        UIController.current.SetDisplayState(showAdditionalInfo.isOn);
    }

    #region Update HP Settings
    public void UpdateNewCurrentHP()
    {
        newCurrentHP = currentHP.text;
    }   
    public void UpdateNewCooldownTime()
    {
        newCooldownTime = cooldownTime.text;
    }
    public void UpdateNewMaxHP()
    {
        newMaxHP = maxHP.text;
    }
    public void OverrideMaxHP()
    {
        maxHP.text = newMaxHP;
        Player.current.maxHP = float.Parse(maxHP.text);
        UIController.current.UpdateHullIntegrety();
    }
    public void OverrideCurrentHP()
    {
        currentHP.text = newCurrentHP;
        DamageControl.current.currentHP = float.Parse(currentHP.text);
        UIController.current.UpdateHullIntegrety();
    }
    public void OverrideCooldownTime()
    {
        cooldownTime.text = newCooldownTime;
        Player.current.cooldownAfterHit = float.Parse(cooldownTime.text);
    }
    public void RefillHealth()
    {
        DamageControl.current.currentHP = Player.current.maxHP;
        currentHP.text = DamageControl.current.currentHP.ToString("F0");
        UIController.current.UpdateHullIntegrety();
    }
    #endregion
    #region Update Movement Settings
    public void OverrideSpeed()
    {
        Player.current.maxSpeed = Mathf.Lerp(0, 20, topSpeedSlider.value);
        topSpeedText.text = "Max Speed: " + (topSpeedSlider.value * 20f).ToString("F2");
    }
    public void OverrideTurnRate()
    {
        Player.current.turnRate = Mathf.Lerp(0, 10, turnRateSlider.value);
        turnRateText.text = "Turn Rate: " + (turnRateSlider.value * 10).ToString("F2");
    }
    public void OverrideAcceleration()
    {
        Player.current.acceleration = Mathf.Lerp(0, 10, accelerationSlider.value);
        accelerationText.text = "Acceleration: " + (accelerationSlider.value * 10).ToString("F2");
    }
    public void OverrideDeceleration()
    {
        Player.current.decceleration = Mathf.Lerp(0, 10, decelerationSlider.value);
        decelerationText.text = "Deceleration: " + (decelerationSlider.value * 10).ToString("F2");
    }
    #endregion
    #region Update Torpedo Settings
    public void UpdateTorpedoFIreRate()
    {
        Player.current.torpedoFireRate = float.Parse(fireRate.text);
    }
    public void UpdateTorpedoLifetime()
    {
        Player.current.timedLifetime = lifetime.isOn;
    }
    public void WhileEditingLifetime()
    {
        Player.current.torpedoLifetime = float.Parse(torpedoLifetime.text);
    }
    public void WhileEditingTorpedoTurnRate()
    {
        Player.current.torpedoTurnRate = float.Parse(torpedoTurnRate.text);
    } 
    public void WhileEditingTorpedoSpeed()
    {
        Player.current.torpedoSpeed = float.Parse(torpedoSpeed.text);
    }    
    public void WhileEditingTorpedoHitRange()
    {
        Player.current.torpedoHitRange = float.Parse(torpedoHitRange.text);
    }
    #endregion
    #region Update Level Settings
    public void UpdateShipRespawnDistance()
    {
        LevelManager.current.shipRespwanDistance = float.Parse(shipRespawnDistance.text);
    }
    public void WhileEditingMapX()
    {
        LevelManager.current.mapWidth = float.Parse(mapSizeX.text);
    }
    public void WhileEditingMapZ()
    {
        LevelManager.current.mapLenght = float.Parse(mapSizeZ.text);
    }

    public void RespawnShips()
    {
        LevelManager.current.DespawnAll();
        LevelManager.current.targetsToSpawn = int.Parse(cargo.text);
        LevelManager.current.minelayersToSpawn = int.Parse(minelayes.text);
        LevelManager.current.huntersToSpawn = int.Parse(hunters.text);
        LevelManager.current.SpawnShips();
    }
    public void DespawnShips()
    {
        LevelManager.current.DespawnAll();
        LevelManager.current.targetsToSpawn = int.Parse(cargo.text);
        LevelManager.current.minelayersToSpawn = int.Parse(minelayes.text);
        LevelManager.current.huntersToSpawn = int.Parse(hunters.text);
    }
    #endregion
    #region Update Mine Settings
    public void RespawnMines()
    {
        LevelManager.current.SpawnMines();
    }
    public void UpdateRespawnDistance()
    {
        float distance = float.Parse(maxDistance.text);
        if(distance -0.1f <= float.Parse(minDistance.text))
        {
            distance = float.Parse(minDistance.text) + 0.1f;
            maxDistance.text = distance.ToString();
        }
        LevelManager.current.maxDistanceFormPlayer = distance;
    }
    public void UpdateMinDistance()
    {
        float distance = float.Parse(minDistance.text);
        if (distance + 0.1f <= float.Parse(maxDistance.text))
        {
            distance = float.Parse(maxDistance.text) - 0.1f;
            minDistance.text = distance.ToString();
        }
        LevelManager.current.spanwSafetyDistance = float.Parse(minDistance.text);
    }
    public void UpdateDensity()
    {
        LevelManager.current.minesToSpawn = int.Parse(minesOnScene.text);
    }
    public void UpdateDamageVariation()
    {        
        damageVariation.text = "Damage amount varition: " + dvSlide.value.ToString("F0") + "%";
        LevelManager.current.mineDamageVariation = dvSlide.value;
    }
    #endregion
    public void UpdateSettingsDisplayed()
    {
        for(int i =0; i < windowParent.childCount; ++i)
        {
            windowParent.GetChild(i).gameObject.SetActive(false);
        }
        settingsWindow.content = (RectTransform)windowParent.GetChild(settingsSelector.value);
        windowParent.GetChild(settingsSelector.value).gameObject.SetActive(true);
        
    }
}
