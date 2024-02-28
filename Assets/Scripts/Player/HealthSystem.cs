using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private  bool DEBUG = false;
    [SerializeField] public  float playerHealth = 100f;
    private  Slider healthBar;
    private bool isTakingDamage = false;
    [SerializeField] private float invincibilityTime = 1f;
    private float _invicibilityTime;
    private AudioSource audioSource;
    [SerializeField] private AudiosClip[] audiosClip;
    private RawImage damageIndicator;
    // Start is called before the first frame update

    [Serializable]
    public struct AudiosClip
    {
        public string name;
        public AudioClip clip;
    }
    
    public void TakeDamage(float damage)
    {
        
        if (!audioSource.isPlaying && !GameController.isPaused)
        {
            SetAudioClip("damage");
            audioSource.Play();
        }
        if (!isTakingDamage)
        {
            damageIndicator.color = new Color(255, 255, 255, 0.25f);
            if (DEBUG) Debug.Log("taking damage");
            playerHealth -= damage;
            isTakingDamage = true;
            UpdateText();
            if (playerHealth <= 0)
            {

                UpdateText();
                Die();
                playerHealth = 0;
            }
        }
    }
    
    public void Heal(float amount)
    {
        playerHealth += amount;
        UpdateText();
    }
    
    private void Die()
    {
        if (DEBUG) Debug.Log("Player Died!");
        SetAudioClip("death");
        audioSource.Play();
        GameObject.Find("GameController").GetComponent<GameController>().Die();

    }

    private void UpdateText()
    {
        healthBar.value = (playerHealth/100f);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
        _invicibilityTime = invincibilityTime;
        damageIndicator = GameObject.Find("DamageIndicator").GetComponent<RawImage>();
        damageIndicator.color = new Color(255, 255, 255, 0f);
    }
    private void Update()
    {
        if (isTakingDamage && !GameController.isPaused)
        {
            _invicibilityTime -= Time.deltaTime;
            if (_invicibilityTime <= 0)
            {
                isTakingDamage = false;
                damageIndicator.color = new Color(255, 255, 255, 0f);
                _invicibilityTime = invincibilityTime;
            }
        }
    }

    private void SetAudioClip(string name) 
    {
        for (int i = 0; i < audiosClip.Length; i++)
        {
            if (audiosClip[i].name == name)
            {
                audioSource.clip = audiosClip[i].clip;
                break;
            }

        }
        
    }
}
