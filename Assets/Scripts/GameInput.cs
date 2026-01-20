using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    public event EventHandler OnInteractAction;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Enable();
        playerInput.Player.Interact.performed += Interact_performed;
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        if (OnInteractAction != null) OnInteractAction(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInput.Player.Move.ReadValue<Vector2>();
        return inputVector.normalized;
    }

    public bool GetInteractButtonPressedThisFrame()
    {
        return playerInput.Player.Interact.WasPressedThisFrame();
    }
}
