using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


/// <summary>
/// This script generates the maze at the start of the game. It simply spawns paths, coins and
/// powerups towards neighbouring waypoints.
/// </summary>
public class MazeGenerator : MonoBehaviour 
{
    public bool spawnCoin = true;
    public bool spawnPowerup = false;

    private HashSet<Waypoint> generatedPaths = new HashSet<Waypoint>();
    private HashSet<Waypoint> generatedCoinTrails = new HashSet<Waypoint>();

	void Awake()
    {
        InstantiatePaths();
        InstantiateCoins();
        InstantiatePowerup();
	}

    private void InstantiatePaths()
    {
        var myWaypoint = GetComponent<Waypoint>();
        MazeManager.Instance.InstantiatePath(myWaypoint.Position, Quaternion.identity);

        foreach (Waypoint otherWaypoint in myWaypoint.ValidNeighbours)
        {
            generatedPaths.Add(otherWaypoint);
            var otherGenerator = otherWaypoint.GetComponent<MazeGenerator>();
            if (!otherGenerator.generatedPaths.Contains(myWaypoint))
            {
                MazeManager.Instance.InstantiatePath(
                   myWaypoint.transform.position, otherWaypoint.transform.position);
            }
        }
    }

    private void InstantiateCoins()
    {
        if (spawnCoin)
            MazeManager.Instance.InstantiateCoin(transform.position);

        var myWaypoint = GetComponent<Waypoint>();
        foreach (Waypoint otherWaypoint in myWaypoint.ValidNeighbours)
        {
            generatedCoinTrails.Add(otherWaypoint);
            var otherGenerator = otherWaypoint.GetComponent<MazeGenerator>();
            if (!otherGenerator.generatedCoinTrails.Contains(myWaypoint) &&
                (this.spawnCoin || otherGenerator.spawnCoin))
            {
                MazeManager.Instance.InstantiateCoinTrail(
                    myWaypoint.transform.position, otherWaypoint.transform.position);
            }
        }
    }

    private void InstantiatePowerup()
    {
        if (spawnPowerup)
            MazeManager.Instance.InstantiatePowerup(transform.position);
    }
}
