using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


/// <summary>
/// Waypoints are the maze's building block. Any two connected waypoints will cause the 
/// maze-generator to spawn a path, coins and (perhaps) powerups between them. The player
/// and ghosts' movements are also constrained by waypoints. Characters can only move from 
/// one waypoint to another.
/// In order to make level-design easier, there is also the Waypoint editor extension, which
/// adds hotkeys and additional graphical interface features to the unity editor to simplify
/// common tasks, such as waypoint extension, connection/disconnection, and mirroring of large
/// waypoint groups in order to create symmetrical levels.
/// </summary>
public class Waypoint : MonoBehaviour
{
    /// <summary>
    /// Indicates whether only ghosts are allowed to visit this waypoint.
    /// </summary>
    public bool ghostOnly = false;
    public Waypoint[] neighbours = new Waypoint[4];

    public Vector2 Position { get { return transform.position; } }

    public IEnumerable<Waypoint> validNeighboursCache = null;
    public IEnumerable<Waypoint> ValidNeighbours
    {
        get 
        {
            if (Application.isPlaying)
            {
                if (validNeighboursCache == null)
                    validNeighboursCache = neighbours.Where(x => x != null);
                
                return validNeighboursCache;
            }
            
            else
                return neighbours.Where(x => x != null); 
        }
    }

    public void AddNeighbour(Waypoint other)
    {
        if (other == null)
            throw new ArgumentNullException("other");
        if (neighbours.Contains(other))
            return;

        int i;
        for (i = 0; i < neighbours.Length && neighbours[i] != null; ++i) ;

        if (i < neighbours.Length)
            neighbours[i] = other;
        else
            Debug.LogError("Attempting to add a neighbour to a full array - " +
                           "increase array capacity in the gameobject or prefab.");
    }

    public bool RemoveNeighbour(Waypoint other)
    {
        int i;
        for (i = 0; i < neighbours.Length && neighbours[i] != other; ++i) ;

        if (i < neighbours.Length)
        {
            neighbours[i] = null;
            return true;
        }
        else
            return false;
    }

    public void Connect(Waypoint other)
    {
        if (other == null)
            throw new ArgumentNullException("other");

        this.AddNeighbour(other);
        other.AddNeighbour(this);
    }

    public void Disconnect(Waypoint other)
    {
        this.RemoveNeighbour(other);
        other.RemoveNeighbour(this);
    }
    
    public bool Accepts(GameObject actor)
    {
        if (actor == null)
            return false;

        return !ghostOnly || (ghostOnly && actor.tag == "Ghost");
    }

    public Waypoint NeighbourAt(
        Vector2 queryDirection, float angleLimit = 360, Func<Waypoint, bool> predicate = null)
    {
        if (queryDirection == Vector2.zero || neighbours.Length == 0)
            return null;

        angleLimit = Mathf.Abs(angleLimit);
        float minAngle = float.MaxValue;
        Waypoint best = null;

        foreach (Waypoint candidate in ValidNeighbours)
        {
            if (predicate != null && !predicate(candidate))
                continue;

            Vector2 candidateDirection = candidate.transform.position - this.transform.position;
            float angle = Mathf.Abs(Vector2.Angle(candidateDirection, queryDirection));

            if (angle < minAngle)
            {
                minAngle = angle;
                best = candidate;
            }
        }

        if (minAngle <= angleLimit)
            return best;
        else
            return null;
    }

    public override string ToString()
    {
        return String.Format("({0}, {1})", transform.position.x, transform.position.y);
    }
}
