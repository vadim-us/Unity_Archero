using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI coins;

    [SerializeField]
    private TextMeshProUGUI hp;

    [SerializeField]
    private TextMeshProUGUI damage;

    [SerializeField]
    private RectTransform menu;

    [SerializeField]
    private RectTransform message;
    public bool IsGameStart { get; set; }

    private bool isGameOnPause;

    //private bool isGameOnFocus;

    public event Action ResetLevel;

    private Animator animator;

    private void Awake()
    {
        Game.Menu += Menu;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (IsGameStart)
        {
            if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Joystick1Button8))
                Menu(!isGameOnPause);
        }
    }

    public void Menu()
    {
        if (IsGameStart)
        {
            Menu(!isGameOnPause);
        }
    }

    public void Menu(bool isShow, Game.gameState state = Game.gameState.start)
    {
        menu.gameObject.SetActive(isShow);

        Pause(isShow);

        if (!isShow)
            Message(false);

        if (state == Game.gameState.win)
        {
            Message(true, "Победа !");
            IsGameStart = false;
        }

        if (state == Game.gameState.end)
        {
            Message(true, "Вы проиграли");
            IsGameStart = false;
        }
    }

    private void Message(bool show, string text = "")
    {
        message.GetComponentInChildren<TextMeshProUGUI>().text = text;
        message.gameObject.SetActive(show);
    }

    private void Pause(bool pause)
    {
        isGameOnPause = pause;
        Time.timeScale = pause ? 0 : 1;
    }

    public void StartOrContinueGame()
    {
        Menu(false);

        if (!IsGameStart)
        {
            ResetLevel?.Invoke();

            StartCoroutine(WaitStart());
        }
    }

    private IEnumerator WaitStart()
    {
        Pause(true);

        int num = 3;
        while (num > 0)
        {
            Message(true, $"Уровень {Game.GetLevel} старт через {num--}");

            yield return new WaitForSecondsRealtime(1);
        }

        Message(false);

        Pause(false);
        IsGameStart = true;
    }

    public void UpdateCoin(int num)
    {
        coins.text = num.ToString();
    }

    public void UpdateHp(int hp, int damage)
    {
        if (damage > 0)
        {
            this.damage.text = (-damage).ToString();
            animator.SetTrigger("damage");
        }
        this.hp.text = hp.ToString();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
