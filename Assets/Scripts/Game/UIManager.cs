using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private TMP_Text healthLabel;
    private TMP_Text speedLabel;
    private RawImage damageIndicator;
    private void Start()
    {
        healthLabel = GameObject.Find("HealthLabel").GetComponent<TMP_Text>();
        speedLabel = GameObject.Find("SpeedLabel").GetComponent<TMP_Text>();
        damageIndicator = GameObject.Find("DamageIndicator").GetComponent<RawImage>();
        damageIndicator.color = new Color(255, 255, 255, 0f);
    }

    private void OnEnable()
    {
        EventManager.Player.OnHealthChanged += UpdateHealth;
        EventManager.Player.OnTakingDamage += TakingDamage;
        EventManager.Player.OnNoTakingDamage += NoTakingDamage;
        EventManager.Player.OnSpeedChanged += UpdateSpeed;
    }

    private void OnDisable()
    {
        EventManager.Player.OnHealthChanged -= UpdateHealth;
        EventManager.Player.OnTakingDamage -= TakingDamage;
        EventManager.Player.OnNoTakingDamage -= NoTakingDamage;
        EventManager.Player.OnSpeedChanged -= UpdateSpeed;
    }


    private void UpdateHealth(Component component, float health)
    {
        healthLabel.SetText(health.ToString());
    }

    private void TakingDamage(Component component)
    {
        damageIndicator.color = new Color(255, 255, 255, 0.25f);
    }

    private void NoTakingDamage(Component component)
    {
        damageIndicator.color = new Color(255, 255, 255, 0f);
    }
    
    private void UpdateSpeed(Component component, float speed)
    {
        speedLabel.SetText(speed.ToString("0.00"));
    }
}
