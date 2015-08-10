using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// The ghost movement strategy is as follows:
///     1. By default, the ghost wanders around the maze pseudo-randomly. When reaching a 
///     waypoint, the ghost randomly chooses a new neighbouring waypoint as its destination. 
///     By preference, the destination waypoint should not equal the last-visited waypoint. 
///     This helps the movement look more natural and avoids situations where the ghost ping-
///     pongs between two waypoints.
///     2. When the player is powered up and in view of the ghost, the ghost will choose as 
///     destination the waypoint that is the opposite direction of the player. By 'having
///     vision' of the player, we mean that a ray traced from the ghost in the player's direction
///     does not intersect anything but the player. The exact tracing rules can be seen at 
///     MazeManager script, in method Raycast().
/// </summary>
public class GhostMovementController : MovementController 
{
    public bool ignorePlayer = false;

    private static readonly float VISION_ANGLE_LIMIT = 0.01f;
    private static LayerMask PLAYER_LAYER_MASK = -1;

    private Waypoint lastVisited;

    protected override void Awake()
    {
        base.Awake();

        if (PLAYER_LAYER_MASK == -1)
            PLAYER_LAYER_MASK = LayerMask.GetMask("Player");
    }

    public override void Respawn()
    {
        base.Respawn();

        lastVisited = spawn;
    }

	void Start() 
    {
        Respawn();
	}

	void FixedUpdate()
    {
        UpdateDestination();
        UpdatePosition();
	}

    private void UpdateDestination()
    {
        if (!ignorePlayer && GameManager.PlayerInteraction.IsPoweredUp && CanSeePlayer())
            RunAway();
        else
            WalkRandomly();
    }

    private void WalkRandomly()
    {
        if (!AtTarget())
            return;

        IEnumerable<Waypoint> candidates =
            Target.ValidNeighbours.Where(x => x.Accepts(this.gameObject));

        DebugUtility.Assert(candidates.Count() > 0);

        if (candidates.Count() > 1)
            candidates = candidates.Where(x => x != lastVisited);

        DebugUtility.Assert(candidates.Count() > 0);

        Waypoint newTarget = RandomUtility.Choose(candidates);
        lastVisited = Target;
        Target = newTarget;
    }

    private void RunAway()
    {
        Vector2 playerPosition = GameManager.Player.transform.position;
        Vector2 runawayDirection = (CurrentPosition - playerPosition).normalized;

        if (AtTarget() ||
            CurrentDirection == Vector2.zero ||
            Vector2.Dot(CurrentDirection, runawayDirection) < 0)
        {
            Waypoint safest = Target.NeighbourAt(runawayDirection);
            DebugUtility.Assert(safest != null);
            Target = safest;
        }
    }

    private bool CanSeePlayer()
    {
        if (!AtTarget())
            return CanSeePlayer(Target);
        else
        {
            foreach (var waypoint in Target.ValidNeighbours)
            {
                if (CanSeePlayer(waypoint))
                    return true;
            }
            return false;
        }
    }

    private bool CanSeePlayer(Waypoint waypoint)
    {
        RaycastHit2D hit = MazeManager.RayCast(
            CurrentPosition, waypoint, waypoint.Position - CurrentPosition,
            VISION_ANGLE_LIMIT, PLAYER_LAYER_MASK);

        return hit.collider != null;
    }
}
