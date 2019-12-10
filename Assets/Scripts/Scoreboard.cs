using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Scoreboard
{
    public List<Tuple<string, int>> scores = new List<Tuple<string, int>>();
    public string currentGameCode = "";

    public List<Tuple<string, int>> GetHighScores(int numberOfScores)
    {
        return scores.OrderByDescending(s => s.Item2).Take(numberOfScores).ToList();
    }

    public void Reset()
    {
        scores.Clear();
        currentGameCode = GenerateNewGameCode();
    }

    public void AddScore(int score)
    {
        scores.Add(new Tuple<string, int>(currentGameCode, score));
        currentGameCode = GenerateNewGameCode();
    }

    private string GenerateNewGameCode()
    {
        string newGameCode;

        do
        {
            newGameCode = "";

            for (int i = 0; i < 3; ++i)
            {
                newGameCode += (char)('A' + UnityEngine.Random.Range(0, 26));
            }
        } while (scores.Any(s => s.Item1 == newGameCode));

        return newGameCode;
    }
}
