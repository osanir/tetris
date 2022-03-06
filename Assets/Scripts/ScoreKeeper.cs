using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] float score = 0f;
    public void AddScore(float value)
    {
        score += value;
        scoreText.text = "Score:\n" + score.ToString("0000000");
    }
}
