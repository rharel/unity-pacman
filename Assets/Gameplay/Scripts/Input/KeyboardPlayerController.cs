using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Issues commands to the player's movement controller based on keyboard input.
/// </summary>
public class KeyboardPlayerController : MonoBehaviour
{
    private PlayerMovementController player;

    void Awake()
    {
        player = GetComponent<PlayerMovementController>();
    }

    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        var inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (inputDirection.sqrMagnitude != 0)
            player.IntendedDirection = inputDirection;
    }
}
