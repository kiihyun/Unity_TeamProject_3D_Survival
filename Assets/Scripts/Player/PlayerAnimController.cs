using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    public Animator animator;

    private void Awake()
    {

    }

    void Start()
    {

    }

    void Update()
    {
        MoveAnim(PlayerManager.Instance.controller.curMoveInput);
    }

    public void MoveAnim(Vector2 _curMoveInput)
    {
        if (PlayerManager.Instance.controller.isRun)
        {
            animator.SetBool("Run", true);
            return;
        }
        else
        {
            animator.SetBool("Run", false);
        }
        animator.SetInteger("Forward", (int)_curMoveInput.y);
        animator.SetInteger("Right", (int)_curMoveInput.x);
    }
}
