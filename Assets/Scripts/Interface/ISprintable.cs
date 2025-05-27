using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface ISprintable 
{
    public void OnSprintInput(InputAction.CallbackContext context);
}
