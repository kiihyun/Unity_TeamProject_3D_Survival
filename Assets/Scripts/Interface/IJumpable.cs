using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IJumpable 
{
    public void OnJumpInput(InputAction.CallbackContext context);
}
