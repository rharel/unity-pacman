using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles global game state (win, loss) and time controls (pause, resume).
/// The game is won when the player collects all coins in the level, and lost when the player
/// dies more than the number of lives assigned at the start of the level.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    public int initialLives = 3;

    public static event Action Pause = delegate { };
    public static event Action Resume = delegate { };
    public static event Action Begin = delegate { };
    public static event Action End = delegate { };
    public static event Action Win = delegate { };
    public static event Action Lose = delegate { };

    public static bool IsPaused { get { return Instance.isPaused; } }

    public static bool GameIsWon 
    { 
        get { return !GameIsLost && Instance.coinsRemaining <= 0; } 
    }

    public static bool GameIsLost
    {
        get { return Instance.livesRemaining < 0; }
    }

    public static bool GameHasEnded
    {
        get { return GameManager.GameIsLost || GameManager.GameIsWon; }
    }

    public static GameObject Player { get { return Instance.player; } }

    public static PlayerInteraction PlayerInteraction { get { return Instance.playerInteraction; } }

    public static int LivesRemaining { get { return Instance.livesRemaining; } }
    public static int CoinsRemaining { get { return Instance.coinsRemaining; } }
    
    private GameObject player;
    private GameObject[] ghosts;

    private MovementController[] actorMovementControllers;

    private PlayerInteraction playerInteraction;
    private int coinsRemaining;
    private int livesRemaining;
    private bool isPaused = false;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        ghosts = GameObject.FindGameObjectsWithTag("Ghost");

        if (player == null)
            Debug.LogWarning("Could not find the player.");
        if (ghosts.Length == 0)
            Debug.LogWarning("Could not find any ghosts.");


        if (player != null)
        {
            playerInteraction = player.GetComponent<PlayerInteraction>();
            playerInteraction.CoinPickup += OnCoinPickup;
            playerInteraction.Death += OnPlayerDeath;

            actorMovementControllers = new MovementController[1 + ghosts.Length];
            actorMovementControllers[0] = player.GetComponent<MovementController>();
        }
        else
            actorMovementControllers = new MovementController[ghosts.Length];

        if (ghosts.Length > 0)
        {
            int j = player != null ? 1 : 0;
            for (int i = 0; i < ghosts.Length; ++i)
                actorMovementControllers[i + j] = ghosts[i].GetComponent<MovementController>();
        }

        coinsRemaining = GameObject.FindGameObjectsWithTag("Coin").Length;
        if (coinsRemaining == 0)
            Debug.LogWarning("Could not find any coins.");

        livesRemaining = initialLives;
        if (livesRemaining < 0)
            Debug.LogWarning("Player is dead at the start of the game.");
    }

    void OnDestroy()
    {
        if (playerInteraction != null)
        {
            playerInteraction.CoinPickup -= OnCoinPickup;
            playerInteraction.Death -= OnPlayerDeath;
        }
    }

    void Start()
    {
        Begin();
    }

    public void PauseGame()
    {
        foreach (var actor in actorMovementControllers)
            actor.speed = 0;

        isPaused = true;
        Pause();
    }

    public void ResumeGame()
    {
        DebugUtility.Assert(!GameHasEnded);

        foreach (var actor in actorMovementControllers)
            actor.ApplyDefaultSpeed();

        isPaused = false;
        Resume();
    }

    private void OnCoinPickup()
    {
        DebugUtility.Assert(coinsRemaining > 0);

        --coinsRemaining;
        if (GameIsWon)
            WinGame();
    }

    private void OnPlayerDeath()
    {
        DebugUtility.Assert(livesRemaining >= 0);

        --livesRemaining;
        if (GameIsLost)
            LoseGame();
    }

    private void WinGame()
    {
        PauseGame();
        Win();
        End();
    }

    private void LoseGame()
    {
        PauseGame();
        Lose();
        End();
    }

    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (!GameHasEnded && Input.GetButtonDown("Pause"))
        {
            if (!IsPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }
}
