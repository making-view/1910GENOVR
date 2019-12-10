using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private GameObject milk;
    [SerializeField] private float milkDepthFactor = 100f;
    [SerializeField] private Text scoreText = null;
    [SerializeField] private Text timerText = null;
    [SerializeField] private Text gameCodeText = null;
    [SerializeField] private VerticalLayoutGroup leaderboardScoresGroup = null;
    [SerializeField] private Font leaderboardFont = null;

    [Header("Game Settings")]
    [SerializeField] private float countDownTime = 5.0f;
    [SerializeField] private float milkingTime = 45.0f;
    
    [HideInInspector] public bool milkingAllowed = false;

    private Scoreboard scoreboard;

    private int score = 0;
    private float timer = 0.0f;
    private float milkDepth = 0.0f;
    private bool gameInProgress = false;
    private string scoreboardPath = "";

    private Vector3 milkStartPos;

    void Start()
    {
        scoreboardPath = Application.persistentDataPath + "/scoreboard.save";
        scoreboard = LoadScores();
        gameCodeText.text = scoreboard.currentGameCode;

        milkStartPos = milk.transform.position;
        milkDepth = milkDepthFactor * 1000f;

        var pushButton = FindObjectOfType<PushButton>();
        pushButton.buttonPressedDelegate += ButtonPressed;
    }

    void Update()
    {
        scoreText.text = score.ToString();
        timerText.text = Mathf.Clamp(timer, 0.0f, milkingTime).ToString("F2");

        var newMilkPos = milkStartPos;
        newMilkPos.y += (score / milkDepth);

        milk.transform.position = newMilkPos;

        if (   OVRInput.GetDown(OVRInput.Button.One)   && OVRInput.GetDown(OVRInput.Button.Two)
            && OVRInput.GetDown(OVRInput.Button.Three) && OVRInput.GetDown(OVRInput.Button.Four))
        {
            StartCoroutine(ResetScoreboard());
        }
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
    }

    private void ButtonPressed()
    {
        if (!gameInProgress)
        {
            StartCoroutine(StartGame());
        }
    }

    private IEnumerator StartGame()
    {
        gameInProgress = true;
        score = 0;
        milk.transform.position = milkStartPos;
        timer = countDownTime;
        gameCodeText.text = scoreboard.currentGameCode;

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

        scoreboard.AddScore(score);
        SaveScores();

        milkingAllowed = false;
        gameInProgress = false;
    }

    private IEnumerator ResetScoreboard()
    {
        var resetTimer = 0.0f;
        var resetTime = 5.0f;

        while (resetTimer < resetTime 
            && OVRInput.GetDown(OVRInput.Button.One)   && OVRInput.GetDown(OVRInput.Button.Two)
            && OVRInput.GetDown(OVRInput.Button.Three) && OVRInput.GetDown(OVRInput.Button.Four))
        {
            resetTimer += Time.deltaTime;

            yield return null;
        }

        if (resetTimer >= resetTime)
        {
            scoreboard.Reset();
            SaveScores();
        }
    }

    private void SaveScores()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(scoreboardPath);
        binaryFormatter.Serialize(file, scoreboard);
        file.Close();

        UpdateLeaderboard();
    }

    private Scoreboard LoadScores()
    {
        var result = new Scoreboard();
        result.Reset();

        if (File.Exists(scoreboardPath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(scoreboardPath, FileMode.Open);
            result = (Scoreboard)binaryFormatter.Deserialize(file);
            file.Close();
        }

        UpdateLeaderboard();

        return result;
    }

    private void UpdateLeaderboard()
    {
        foreach(Transform child in leaderboardScoresGroup.transform)
        {
            Destroy(child.gameObject);
        }

        var highSchores = scoreboard.GetHighScores(10);

        for (int scoreNo = 1; scoreNo <= highSchores.Count; ++scoreNo)
        {
            var newScore = new GameObject();
            var text = newScore.AddComponent<Text>();
            text.font = leaderboardFont;
            text.fontStyle = FontStyle.Bold;
            text.fontSize = 58;
            text.color = Color.black;

            var tabs = scoreNo == highSchores.Count ? "\t\t" : "\t";
            text.text = scoreNo + "." + tabs + "[" + highSchores[scoreNo - 1].Item1 + "] " + highSchores[scoreNo - 1].Item2; 
        }
    }
}