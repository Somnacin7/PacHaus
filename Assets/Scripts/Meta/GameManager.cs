using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    public Vector2 playerInitialPosition = Vector2.zero;

    private ScoreManager scoreManager;

    private GameObject player = null;
    private DeathEvents playerDeathEvents = null;
    private PacdotPickup playerPacdotPickupEvents = null;

    public void Awake() {
        // If there is an instance and it isn't this one, then commit suicide
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }

        scoreManager = GetComponent<ScoreManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) {
            throw new Exception("Error: no player was found in the scene");
        } else {
            playerDeathEvents        = player.GetComponent<DeathEvents>();
            playerPacdotPickupEvents = player.GetComponent<PacdotPickup>();
        }
    }

    void OnEnable() {
        if (playerDeathEvents != null) {
            playerDeathEvents.OnDeath            += OnPlayerDeath;
        }

        if (playerPacdotPickupEvents != null) {
            playerPacdotPickupEvents.OnDotPickup += OnPlayerPacdotPickup;
        }
    }

    void OnDisable() {
        if (playerDeathEvents != null) {
            playerDeathEvents.OnDeath            -= OnPlayerDeath;
        }

        if (playerPacdotPickupEvents != null) {
            playerPacdotPickupEvents.OnDotPickup -= OnPlayerPacdotPickup;
        }
    }

    void OnPlayerDeath() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    void OnPlayerPacdotPickup() {
        scoreManager.AddScore(5.0f);
    }
}
