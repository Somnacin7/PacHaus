using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour {

    public Text scoreText;
    public string scorePrefix = "SCORE: ";

    public delegate void ScoreChange();
    public event ScoreChange OnScoreChange;

    public float currentScore;

    private static ScoreManager _instance = null;
    public static ScoreManager Instance {  get { return _instance; } }

    // Start() is called only once per script (regardless of LoadScene())
    void Start() {
        currentScore = 0f;
    }

    void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
        Setup();
    }

    void Setup() {
        if (scoreText == null) {
            var go = GameObject.FindGameObjectWithTag("scoreText");
            var text = go.GetComponent<Text>();

            if (text != null) {
                scoreText = text;
            } else {
                throw new Exception("No Score Text Found");
            }
        }
    }

    void OnSceneLoaded(Scene prevScene, LoadSceneMode mode) {
        Setup();
    }

    void OnSceneUnloaded(Scene prevScene) {
        Setup();
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

    public void SetScore(float newScore) {
        currentScore = newScore;

        if (OnScoreChange != null) {
            OnScoreChange();
        }
    }
}
    