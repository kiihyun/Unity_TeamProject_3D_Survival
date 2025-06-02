using UnityEngine;
using UnityEngine.UI;

public class PlayerConditionUI : MonoBehaviour
{
    [Header("Radial Condition UI")]
    public Image healthCircle;
    public Image staminaCircle;
    public Image hungerCircle;
    public Image thirstCircle;

    private PlayerCondition playerCondition;

    private void Start()
    {
        playerCondition = FindObjectOfType<PlayerCondition>();
    }

    private void Update()
    {
        if (playerCondition == null) return;

        // fillAmount = 현재값 / 최대값
        healthCircle.fillAmount = playerCondition.Health / playerCondition.maxHealth;
        staminaCircle.fillAmount = playerCondition.Stamina / playerCondition.maxStamina;
        hungerCircle.fillAmount = playerCondition.Hunger / playerCondition.maxHunger;
        thirstCircle.fillAmount = playerCondition.Thirst / playerCondition.maxThirst;
    }
}
