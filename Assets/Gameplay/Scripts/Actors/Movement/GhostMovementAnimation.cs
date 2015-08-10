using UnityEngine;
using System.Collections;

public class GhostMovementAnimation : StateMachineBehaviour 
{
    private static readonly string DIRECTION_X = "Direction.X";
    private static readonly string DIRECTION_Y = "Direction.Y";

    private GhostMovementController ghost;

    override public void OnStateEnter(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ghost = animator.GetComponent<GhostMovementController>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger(DIRECTION_X, Mathf.RoundToInt(ghost.CurrentDirection.x));
        animator.SetInteger(DIRECTION_Y, Mathf.RoundToInt(ghost.CurrentDirection.y));
    }
}
