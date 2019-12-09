using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private GameObject milk;
    [SerializeField] private float milkDepthFactor = 100f;
    [SerializeField] private Text scoreText = null;
    [SerializeField] private Text timerText = null;

    [Header("Game Settings")]
    [SerializeField] private float countDownTime = 5.0f;
    [SerializeField] private float milkingTime = 45.0f;
    
    [HideInInspector] public bool milkingAllowed = false;

    private int score = 0;
    private float timer = 0.0f;
    private float milkDepth = 0.0f;
    private bool gameInProgress = false;

    private Vector3 milkStartPos;

    void Start()
    {
        milkStartPos = milk.transform.position;
        milkDepth = milkDepthFactor * 1000f;

        var pushButton = FindObjectOfType<PushButton>();
        pushButton.buttonPressedDelegate += ButtonPressed;
    }

    void Update()
    {
        scoreText.text = score.ToString();
        timerText.text = Mathf.Clamp(timer, 0.0f, milkingTime).ToString();

        var newMilkPos = milkStartPos;
        newMilkPos.y += (score / milkDepth);

        milk.transform.position = newMilkPos;
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    void ButtonPressed()
    {
        if (!gameInProgress)
        {
            StartCoroutine(StartGame());
        }
    }

    IEnumerator StartGame()
    {
        gameInProgress = true;
        score = 0;
        milk.transform.position = milkStartPos;
        timer = countDownTime;

        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;

            yield return null;
        }

        timer = milkingTime;
        milkingAllowed = true;

        while(timer > 0.0f)
        {
            timer -= Time.deltaTime;

            yield return null;
        }

        milkingAllowed = false;
        gameInProgress = false;
    }
}