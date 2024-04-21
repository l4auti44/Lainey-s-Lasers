using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private  bool DEBUG = false;
    [SerializeField] public  float playerHealth = 100f;
    private bool isTakingDamage = false;
    [SerializeField] private float invincibilityTime = 1f;
    private float _invicibilityTime;
    


    private void Start()
    {
        _invicibilityTime = invincibilityTime;
        
        
    }
    private void Update()
    {
        if (isTakingDamage && !GameController.isPaused)
        {
            _invicibilityTime -= Time.deltaTime;
            if (_invicibilityTime <= 0)
            {
                isTakingDamage = false;
                EventManager.Player.OnNoTakingDamage.Invoke(this);
                _invicibilityTime = invincibilityTime;
            }
        }
    }

    public void TakeDamage(float damage)
    {

        EventManager.Player.OnTakingDamage.Invoke(this);
        if (DEBUG) Debug.Log("taking damage");
        playerHealth -= damage;
        if (playerHealth < 0)
        {
            playerHealth = 0;
        }
        EventManager.Player.OnHealthChanged.Invoke(this, playerHealth);
        isTakingDamage = true;
        if (playerHealth <= 0)
        {
            Die();
            playerHealth = 0;
        }
        
    }
    
    public void Heal(float amount)
    {
        playerHealth += amount;

    }
    
    private void Die()
    {
        if (DEBUG) Debug.Log("Player Died!");

        GameObject.Find("GameManager").GetComponent<GameController>().Die();

    }

}
