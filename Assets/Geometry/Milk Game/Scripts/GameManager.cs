using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject milk;
    [SerializeField] private float milkDepthFactor = 100f;

    private int score = 0;
    private float milkDepth;

    private Vector3 milkStartPos;

    void Start()
    {
        milkStartPos = milk.transform.position;
        milkDepth = milkDepthFactor * 1000f;
    }

    void Update()
    {
        var newMilkPos = milkStartPos;
        newMilkPos.y += (score / milkDepth);

        milk.transform.position = newMilkPos;
    }

    public void AddScore(int amount)
    {
        score += amount;
    }
}