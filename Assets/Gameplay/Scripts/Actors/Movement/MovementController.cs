using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// This class forms the basis for both the player movement as well as ghost movement behaviors.
/// Characters have a spawn point and a 'speed' value, indicating how fast they move. When moving,
/// characters are always advance towards a target waypoint. If they have no target or the target
/// has already been reached, the controller does nothing until a new target is specified.
/// </summary>
public abstract class MovementController : MonoBehaviour 
{
    public Waypoint spawn;
    public float speed = 1.0f;

    private Vector2 currentDirection = Vector2.zero;
    public Vector2 CurrentDirection
    {
        get { return currentDirection; }
        private set { currentDirection = value.normalized; }
    }
   
    protected Vector2 CurrentPosition 
    { 
        get { return this.transform.position; }
        set { this.transform.position = value; }
    }
    
    private Waypoint target;
    protected Waypoint Target
    {
        get { return target; }
        set { target = value; }
    }

    private float defaultSpeed;

    protected virtual void Awake()
    {
        // We save the default speed value because when the game is paused,
        // all character's movement speed is set to zero. When resuming gameplay,
        // we need a reference to the characters original intended speed.
        defaultSpeed = speed;
    }

    public void ApplyDefaultSpeed()
    {
        speed = defaultSpeed;
    }

    public virtual void Respawn()
    {
        CurrentPosition = spawn.Position;
        CurrentDirection = Vector2.zero;
        transform.rotation = Quaternion.identity;

        Target = spawn;
    }
    
    protected void UpdatePosition()
    {
        if (Target == null || AtTarget())
            return;

        float step = speed * Time.fixedDeltaTime;
        CurrentDirection = Target.Position - CurrentPosition;
        CurrentPosition = Vector2.MoveTowards(CurrentPosition, Target.Position, step);
    }

    protected bool AtTarget()
    {
        DebugUtility.Assert(Target != null);

        return CurrentPosition == Target.Position;
    }
}
