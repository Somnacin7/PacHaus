using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Text livesText;
    public string livesPrefix = "";

    public int numberOfLives = 3;
    private int currentLives = 0;

    private int count = 0;

    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    private ScoreManager scoreManager;

    private GameObject player = null;
    private DeathEvents playerDeathEvents = null;
    private PacdotPickup playerPacdotPickupEvents = null;

    void Start() {
        Setup();
        currentLives = numberOfLives;
    }

    void Awake() {
        // If there is an instance and it isn't this one, then commit suicide
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    void Setup() {
        scoreManager = GetComponent<ScoreManager>();
        if (scoreManager == null) {
            Debug.LogWarning("No score manager on game manger");
        }

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) {
            throw new Exception("Error: no player was found in the scene");
        } else {
            playerDeathEvents = player.GetComponent<DeathEvents>();
            playerPacdotPickupEvents = player.GetComponent<PacdotPickup>();
        }

    }

    void OnEnable() {
        RegisterEvents();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDisable() {
        DeregisterEvents();
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    void OnSceneLoaded(Scene prevScene, LoadSceneMode mode) {
        Debug.Log("count: " + ++count);
        Setup();
        RegisterEvents();
    }

    void OnSceneUnloaded(Scene prevScene) {
        currentLives--;
        DeregisterEvents();
    }

    void OnPlayerDeath() {
        if (currentLives > 0) {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        } else {
            Time.timeScale = 0;
        }
    }

    void OnPlayerPacdotPickup() {
        scoreManager.AddScore(5.0f);
    }

    void RegisterEvents() {
        if (playerDeathEvents != null) {
            playerDeathEvents.OnDeath += OnPlayerDeath;
        }

        if (playerPacdotPickupEvents != null) {
            playerPacdotPickupEvents.OnDotPickup += OnPlayerPacdotPickup;
        }
    }

    void DeregisterEvents() {
        if(playerDeathEvents != null) {
            playerDeathEvents.OnDeath -= OnPlayerDeath;
        }

        if (playerPacdotPickupEvents != null) {
            playerPacdotPickupEvents.OnDotPickup -= OnPlayerPacdotPickup;
        }
    }

    void Update() {
        livesText.text = livesPrefix + currentLives;
    }
}
