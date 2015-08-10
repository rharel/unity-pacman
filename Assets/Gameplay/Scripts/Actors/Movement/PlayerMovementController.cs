using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The player's movement is controlled by a single 'intended direction' vector.
/// This vectors roughly hints at what direction the player wishes to go, and this controller
/// then chooses a waypoint that most closely aligns with this direction.
/// </summary>
public class PlayerMovementController : MovementController 
{
    private static readonly Vector2 DEFAULT_DIRECTION = Vector2.right;
    private static readonly float DIRECTION_SEEK_ANGLE_LIMIT = 5;

    private Vector2 intendedDirection = Vector2.zero;
    public Vector2 IntendedDirection
    {
        get { return intendedDirection; }
        set { intendedDirection = value.normalized; }
    }

    void Start()
    {
        Respawn();
    }

    public override void Respawn()
    {
        base.Respawn();

        IntendedDirection = Vector2.zero;
    }

	void FixedUpdate() 
    {
        UpdateDestination();
        UpdatePosition();
        UpdateRotation();
	}

    private void UpdateDestination()
    {
        DebugUtility.Assert(Target != null);

        if (AtTarget())
        {
            Waypoint intendedTarget = NeighbourAt(Target, IntendedDirection);

            if (intendedTarget == null)
                intendedTarget = NeighbourAt(Target, CurrentDirection);
            if (intendedTarget == null)
                IntendedDirection = Vector2.zero;
            else
                Target = intendedTarget;
        }

        else if (IntendedDirection == -CurrentDirection)
        {
            Target = NeighbourAt(Target, IntendedDirection);
            DebugUtility.Assert(Target != null);
        }
    }

    private void UpdateRotation()
    {
        if (CurrentDirection != Vector2.zero)
            transform.rotation = Quaternion.FromToRotation(DEFAULT_DIRECTION, CurrentDirection);
    }

    private Waypoint NeighbourAt(Waypoint waypoint, Vector2 direction)
    {
        return waypoint.NeighbourAt(
            direction, DIRECTION_SEEK_ANGLE_LIMIT, x => x.Accepts(this.gameObject));
    }
}
