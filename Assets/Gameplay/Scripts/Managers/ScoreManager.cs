using UnityEngine;
using System.Collections;


/// <summary>
/// Tracks the player's score as the game progresses.
/// </summary>
public class ScoreManager : Singleton<ScoreManager>
{
    public int coinValue = 100;
    public int ghostValue = 500;
    public int deathPenalty = 1000;

    public static int Score { get { return Instance.score; } }
    public static int CoinsCollected { get { return Instance.coinsCollected; } }
    public static int GhostsKilled { get { return Instance.ghostsKilled; } }
    public static int PlayerDeaths { get { return Instance.playerDeaths; } }
    
    private int score = 0;
    private int coinsCollected = 0;
    private int ghostsKilled = 0;
    private int playerDeaths = 0;

    void Start()
    {
        GameManager.PlayerInteraction.CoinPickup += OnCoinPickup;
        GameManager.PlayerInteraction.GhostKill += OnGhostKill;
        GameManager.PlayerInteraction.Death += OnPlayerDeath;
    }

    void OnDestroy()
    {
        if (!GameManager.Exists || GameManager.PlayerInteraction == null)
            return;

        GameManager.PlayerInteraction.CoinPickup -= OnCoinPickup;
        GameManager.PlayerInteraction.GhostKill -= OnGhostKill;
        GameManager.PlayerInteraction.Death -= OnPlayerDeath;
    }

    private void OnCoinPickup()
    {
        score += coinValue;
        ++coinsCollected;
    }

    private void OnGhostKill()
    {
        score += ghostValue;
        ++ghostsKilled;
    }

    private void OnPlayerDeath()
    {
        score -= deathPenalty;
        ++playerDeaths;
    }
}
