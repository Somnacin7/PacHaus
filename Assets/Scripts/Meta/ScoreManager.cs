using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public Text scoreText;
    public string scorePrefix = "SCORE: ";

    public delegate void ScoreChange();
    public event ScoreChange OnScoreChange;

    public float currentScore;

    void Start() {
        currentScore = 0f;
    }

    void Update() {
        scoreText.text = scorePrefix + currentScore;
    }

    public void Reset() {
        currentScore = 0f;
        OnScoreChange();
    }

    public void AddScore(float scoreToAdd) {
        currentScore += scoreToAdd;

        if (OnScoreChange != null) {
            OnScoreChange();
        }
    }
}
    