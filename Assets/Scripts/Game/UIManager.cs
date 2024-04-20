using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private TMP_Text healthLabel;
    private TMP_Text speedLabel;
    private TMP_Text titlePauseMenu;
    private RawImage damageIndicator;
    private GameObject lainey;
    private RawImage[] laineyStates;
    private GameObject resumeButton;

    private void Awake()
    {
        resumeButton = GameObject.Find("ResumeButton");
        titlePauseMenu = GameObject.Find("TitlePauseMenu").GetComponent<TMP_Text>();
    }
    private void Start()
    {
        
        lainey = GameObject.Find("Lainey");
        GetLaineyImages();
        DisableLaineyImages();
        healthLabel = GameObject.Find("HealthLabel").GetComponent<TMP_Text>();
        speedLabel = GameObject.Find("SpeedLabel").GetComponent<TMP_Text>();
        damageIndicator = GameObject.Find("DamageIndicator").GetComponent<RawImage>();
        damageIndicator.color = new Color(255, 255, 255, 0f);
    }

    private void GetLaineyImages()
    {
        laineyStates = new RawImage[lainey.transform.childCount];
        for (int i = 0; i < lainey.transform.childCount; i++)
        {
            laineyStates[i] = lainey.transform.GetChild(i).GetComponent<RawImage>();
        }
    }

    private void DisableLaineyImages()
    {
        for (int i = 0; i < laineyStates.Length; i++)
        {
            laineyStates[i].gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        EventManager.Player.OnHealthChanged += UpdateHealth;
        EventManager.Player.OnTakingDamage += TakingDamage;
        EventManager.Player.OnNoTakingDamage += NoTakingDamage;
        EventManager.Player.OnSpeedChanged += UpdateSpeed;
        EventManager.Player.OnMovementStateChanged += UpdateLaineyImage;
        
        EventManager.Game.OnWin += UpdateWinText;
        EventManager.Game.OnHitless += HitlessCondition;
    }

    private void OnDisable()
    {
        EventManager.Player.OnHealthChanged -= UpdateHealth;
        EventManager.Player.OnTakingDamage -= TakingDamage;
        EventManager.Player.OnNoTakingDamage -= NoTakingDamage;
        EventManager.Player.OnSpeedChanged -= UpdateSpeed;
        EventManager.Player.OnMovementStateChanged -= UpdateLaineyImage;

        EventManager.Game.OnWin -= UpdateWinText;
        EventManager.Game.OnHitless -= HitlessCondition;
    }

    private void HitlessCondition(Component component)
    {
        DisableLaineyImages();
        laineyStates[5].gameObject.SetActive(true);
    }
    private void UpdateWinText(Component component)
    {
        titlePauseMenu.SetText("YOU WIN!");
        resumeButton.SetActive(false);
    }

    private void UpdateHealth(Component component, float health)
    {
        healthLabel.SetText(health.ToString());
        if (health <= 0)
        {
            DisableLaineyImages();
            laineyStates[4].gameObject.SetActive(true);
        }
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

    private void UpdateLaineyImage(Component component, PlayerMovement.MovementState state)
    {
        DisableLaineyImages();
        switch(state)
        {
            case PlayerMovement.MovementState.idle:
                laineyStates[0].gameObject.SetActive(true);
                break;

            case PlayerMovement.MovementState.walking:
                laineyStates[1].gameObject.SetActive(true);
                break;

            case PlayerMovement.MovementState.crouching:
                laineyStates[2].gameObject.SetActive(true);
                break;

            case PlayerMovement.MovementState.air:
                laineyStates[3].gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
}
