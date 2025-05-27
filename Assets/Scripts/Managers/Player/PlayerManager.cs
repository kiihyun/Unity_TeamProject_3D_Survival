using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public GameObject player;
    public PlayerController controller;
    public PlayerCondition condition;
    public PlayerInteraction interaction;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (!player.TryGetComponent<PlayerController>(out controller))
        {
            Debug.LogError("PlayerController component is missing on the player GameObject.");
        }
        if (!player.TryGetComponent<PlayerCondition>(out condition))
        {
            Debug.LogError("PlayerCondition component is missing on the player GameObject.");
        }
        if (!player.TryGetComponent<PlayerInteraction>(out interaction))
        {
            Debug.LogError("PlayerInteraction component is missing on the player GameObject.");
        }
    }
}

