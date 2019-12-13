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
    [SerializeField] private float countDownTime = 3.0f;
    [SerializeField] private float milkingTime = 45.0f;
    
    [HideInInspector] public bool milkingAllowed = false;

    private Scoreboard scoreboard;
    private AudioSource audioSource;

    private int score = 0;
    private float timer = 0.0f;
    private float milkDepth = 0.0f;
    private bool gameInProgress = false;
    private string scoreboardPath = "";
    private bool tryingReset = false;

    private Vector3 milkStartPos;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        scoreboardPath = Application.persistentDataPath + "/scoreboard.save";
        scoreboard = LoadScores();
        UpdateLeaderboard();
        gameCodeText.text = scoreboard.currentGameCode;

        milkStartPos = milk.transform.position;
        milkDepth = milkDepthFactor * 1000f;

        var pushButton = FindObjectOfType<PushButton>();
        pushButton.buttonPressedDelegate += ButtonPressed;
    }

    void Update()
    {
        scoreText.text = score.ToString();

        var newMilkPos = milkStartPos;
        newMilkPos.y += (score / milkDepth);

        milk.transform.position = newMilkPos;

        if (!tryingReset && OVRInput.Get(OVRInput.Button.One)   && OVRInput.Get(OVRInput.Button.Two)
                         && OVRInput.Get(OVRInput.Button.Three) && OVRInput.Get(OVRInput.Button.Four))
        {
            StartCoroutine(ResetScoreboard());
        }
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
    }

    public int getScore()
    {
        return score;
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
        audioSource.Stop();
        audioSource.Play();
        gameInProgress = true;
        score = 0;
        milk.transform.position = milkStartPos;
        timer = 0;
        gameCodeText.text = scoreboard.currentGameCode;

        while (timer <= countDownTime)
        {
            timer += Time.deltaTime;
            timerText.text = ((int)timer).ToString();

            yield return null;
        }

        timer = milkingTime;
        milkingAllowed = true;

        while(timer > 0.0f)
        {
            timer -= Time.deltaTime;
            timerText.text = Mathf.Clamp(timer, 0.0f, milkingTime).ToString("F2");

            yield return null;
        }

        timerText.text = Mathf.Clamp(timer, 0.0f, milkingTime).ToString("F2");
        scoreboard.AddScore(score);
        SaveScores();

        milkingAllowed = false;
        gameInProgress = false;
    }

    private IEnumerator ResetScoreboard()
    {
        tryingReset = true;
        var resetTimer = 0.0f;
        var resetTime = 5.0f;

        while (resetTimer < resetTime 
            && OVRInput.Get(OVRInput.Button.One)   && OVRInput.Get(OVRInput.Button.Two)
            && OVRInput.Get(OVRInput.Button.Three) && OVRInput.Get(OVRInput.Button.Four))
        {
            resetTimer += Time.deltaTime;

            yield return null;
        }

        if (resetTimer >= resetTime)
        {
            scoreboard.Reset();
            SaveScores();
        }

        tryingReset = false;
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
            newScore.name = "Score " + scoreNo;

            var text = newScore.AddComponent<Text>();
            text.font = leaderboardFont;
            text.fontStyle = FontStyle.Bold;
            text.fontSize = 58;
            text.color = Color.black;

            var tabs = scoreNo > 9 ? "\t" : "\t\t";
            text.text = scoreNo + "." + tabs + "[" + highSchores[scoreNo - 1].Item1 + "] " + highSchores[scoreNo - 1].Item2;

            newScore.transform.position = leaderboardScoresGroup.transform.position;
            newScore.transform.rotation = leaderboardScoresGroup.transform.rotation;
            newScore.transform.parent = leaderboardScoresGroup.transform;
            newScore.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}