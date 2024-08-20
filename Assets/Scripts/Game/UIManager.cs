using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    private TMP_Text healthLabel;
    private TMP_Text speedLabel;
    private TMP_Text stateLabel;
    private TMP_Text titlePauseMenu;
    private RawImage damageIndicator;
    private GameObject lainey;
    private RawImage[] laineyStates;
    private GameObject resumeButton;
    private GameObject restartButton;
    private Slider healthBar;
    [SerializeField] private GameObject[] tutorials;

    private void Awake()
    {
        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
        resumeButton = GameObject.Find("ResumeButton");
        restartButton = GameObject.Find("RestartButton");
        titlePauseMenu = GameObject.Find("TitlePauseMenu").GetComponent<TMP_Text>();
        if (tutorials.Length > 0)
        {
            foreach(var tuto in tutorials)
            {
                tuto.SetActive(false);
            }
        }
    }
    private void Start()
    {
        lainey = GameObject.Find("Lainey");
        GetLaineyImages();
        DisableLaineyImages();
        healthLabel = GameObject.Find("HealthLabel").GetComponent<TMP_Text>();
        speedLabel = GameObject.Find("SpeedLabel").GetComponent<TMP_Text>();
        stateLabel = GameObject.Find("StateLabel").GetComponent<TMP_Text>();
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
        EventManager.Game.OnDie += UpdateTextOnDie;
        EventManager.Game.OnHitless += HitlessCondition;
        EventManager.Game.OnTutorialTrigger += UpdateTutorial;
    }

    private void OnDisable()
    {
        EventManager.Player.OnHealthChanged -= UpdateHealth;
        EventManager.Player.OnTakingDamage -= TakingDamage;
        EventManager.Player.OnNoTakingDamage -= NoTakingDamage;
        EventManager.Player.OnSpeedChanged -= UpdateSpeed;
        EventManager.Player.OnMovementStateChanged -= UpdateLaineyImage;

        EventManager.Game.OnWin -= UpdateWinText;
        EventManager.Game.OnDie -= UpdateTextOnDie;
        EventManager.Game.OnHitless -= HitlessCondition;
        EventManager.Game.OnTutorialTrigger -= UpdateTutorial;
    }

    private void HitlessCondition(Component component)
    {
        DisableLaineyImages();
        laineyStates[5].gameObject.SetActive(true);
    }
    private void UpdateWinText(Component component)
    {
        EventSystem.current.SetSelectedGameObject(restartButton);
        resumeButton.GetComponent<Button>().interactable = false;
        titlePauseMenu.SetText("YOU WIN!");

    }

    private void UpdateHealth(Component component, float health)
    {
        healthLabel.SetText(health.ToString());
        healthBar.value = health / 100f;


        if (health <= 0)
        {
            DisableLaineyImages();
            laineyStates[4].gameObject.SetActive(true);
        }
    }

    private void TakingDamage(Component component)
    {
        damageIndicator.color = new Color(255, 255, 255, 0.4f);
    }

    private void NoTakingDamage(Component component)
    {
        damageIndicator.color = new Color(255, 255, 255, 0f);
    }
    
    private void UpdateSpeed(Component component, float speed)
    {
        speedLabel.SetText(speed.ToString("0.00"));
    }

    private void UpdateTutorial(Component component, int num)
    {
        foreach (var tutorial in tutorials)
        {
            tutorial.SetActive(false);
        }
        if (num != -1)
        {
            tutorials[num].SetActive(true);
        }
        
    }

    private void UpdateTextOnDie(Component component)
    {
        titlePauseMenu.SetText("YOU DIED!");
    }

    private void UpdateLaineyImage(Component component, PlayerMovementAdvanced.MovementState state)
    {
        DisableLaineyImages();
        switch(state)
        {
            case PlayerMovementAdvanced.MovementState.idle:
                laineyStates[0].gameObject.SetActive(true);
                stateLabel.SetText("Idle");
                break;

            case PlayerMovementAdvanced.MovementState.walking:
                laineyStates[1].gameObject.SetActive(true);
                stateLabel.SetText("Walking");
                break;

            case PlayerMovementAdvanced.MovementState.crouching:
                laineyStates[2].gameObject.SetActive(true);
                stateLabel.SetText("Crouching");
                break;

            case PlayerMovementAdvanced.MovementState.air:
                laineyStates[3].gameObject.SetActive(true);
                stateLabel.SetText("Air");
                break;
            case PlayerMovementAdvanced.MovementState.sliding:
                laineyStates[2].gameObject.SetActive(true);
                stateLabel.SetText("Sliding");
                break;
            default:
                break;
        }
    }
}
