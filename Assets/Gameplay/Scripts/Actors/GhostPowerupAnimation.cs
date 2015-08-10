using UnityEngine;
using System.Collections;

public class GhostPowerupAnimation : StateMachineBehaviour 
{
    private static readonly string POWERUP = "Powerup On";
    
    private Animator animator;
    
	override public void OnStateEnter(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        this.animator = animator;
        GameManager.PlayerInteraction.PowerupOn += OnPlayerPowerupOn;
        GameManager.PlayerInteraction.PowerupOff += OnPlayerPowerupOff;
	}

    override public void OnStateExit(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.PlayerInteraction.PowerupOn -= OnPlayerPowerupOn;
        GameManager.PlayerInteraction.PowerupOff -= OnPlayerPowerupOff;
    }

    private void OnPlayerPowerupOn()
    {
        animator.SetBool(POWERUP, true);
    }

    private void OnPlayerPowerupOff()
    {
        animator.SetBool(POWERUP, false);
    }
}
