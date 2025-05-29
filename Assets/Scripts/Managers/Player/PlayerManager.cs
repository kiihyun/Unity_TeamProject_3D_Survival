using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public GameObject player;
    private PlayerController Controller;
    private PlayerAnimController Animator;
    private PlayerCondition Condition;
    private PlayerInteraction Interaction;
    private FootStep FootStep;

    public PlayerController controller
    {
        get; private set;
    }
    public PlayerAnimController animator
    {
        get; private set;
    }
    public PlayerCondition condition
    {
        get; private set;
    }
    public PlayerInteraction interaction
    {
        get; private set;
    }
    public FootStep footStep
    {
        get; private set;
    }

    private void Start()
    {
        player = this.gameObject;

        if (!TryGetComponent<PlayerController>(out Controller))
        {
            Debug.LogError("PlayerController component is missing on the player GameObject.");
        }
        if (!TryGetComponent<PlayerCondition>(out Condition))
        {
            Debug.LogError("PlayerCondition component is missing on the player GameObject.");
        }
        if (!TryGetComponent<PlayerInteraction>(out Interaction))
        {
            Debug.LogError("PlayerInteraction component is missing on the player GameObject.");
        }

        FootStep = GetComponentInChildren<FootStep>();
        Animator = GetComponentInChildren<PlayerAnimController>();
    }
}

