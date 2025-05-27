using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCondition : MonoBehaviour, IDamagable
{
    [Header("Health")]
    [SerializeField] public float health;
    public float minHealth = 0f; //최소 체력
    public float maxHealth; //최대 체력
    public float healthRegenRate; //체력 회복 속도
    public float healthDegenRate; //체력 감소 속도
    public float Health 
    {
        get { return health; } 
        set 
        {
            health = Mathf.Clamp(value, 0, maxHealth); 
        }
    }

    [Header("Stamina")]
    [SerializeField] public float stamina;
    public float minStamina = 0f; //최소 스태미나
    public float maxStamina; //최대 스태미나
    public float staminaRegenRate = 0.1f; //스태미나 회복 속도
    public float staminaDegenRate = 0.1f; //스태미나 감소 속도
    public float Stamina 
    {
        get { return stamina; } 
        set 
        {
            stamina = Mathf.Clamp(value, 0, maxStamina); 
        }
    }

    [Header("Hunger")]
    [SerializeField] public float hunger;
    public float minHunger = 0f; //최소 배고픔
    public float maxHunger;//최대 배고픔
    public float hungerToHeal;
    public float Hunger 
    {
        get { return hunger; } 
        set 
        {
            hunger = Mathf.Clamp(value, 0, maxHunger); 
        }
    }

    [Header("Thirst")]
    [SerializeField] public float thirst;
    public float minThirst = 0f; //최소 목마름
    public float maxThirst; //최대 목마름
    public float thirstToHeal;
    public float Thirst 
    {
        get { return thirst; } 
        set 
        {
            thirst = Mathf.Clamp(value, 0, maxThirst); 
        }
    }

    [Header("body Temperature")]
    [SerializeField] public float bodyTemp;
    public const float minBodyTemp = 33f; //최소 체온
    public const float maxBodyTemp = 40f; //최대 체온
    public float minNormalBodyTemp = 35f; //최소 정상 체온
    public float maxNormalBodyTemp = 38f; //최대 정상 체온

    public float BodyTemp 
    {
        get { return bodyTemp; } 
        set 
        {
            bodyTemp = Mathf.Clamp(value, minBodyTemp, maxBodyTemp); 
        } 
    }


    public Slider healthUI;
    public Slider staminaUI;
    public Slider hungerUI;
    public Slider thirstUI;
    public TextMeshProUGUI tempUI;


    void Start()
    {
        health = maxHealth; //초기 체력 설정
        stamina = maxStamina; //초기 스태미나 설정
        hunger = maxHunger; //초기 배고픔 설정
        thirst = maxHunger; //초기 목마름 설정
        bodyTemp = 36.5f; //초기 체온 설정 (정상 범위 내에서 설정)
    }

    void Update()
    {
        UpdateConditions();
        TestUI(); //UI 업데이트 메소드 호출 (테스트용)
    }

    //회복 조건

    public void TestUI()
    {
        healthUI.value = health / maxHealth; //체력 UI 업데이트
        staminaUI.value = stamina / maxStamina; //스태미나 UI 업데이트
        hungerUI.value = hunger / maxHunger; //배고픔 UI 업데이트
        thirstUI.value = thirst / maxThirst; //목마름 UI 업데이트
        tempUI.text = $"Temp: {bodyTemp:F1}°C"; //체온 UI 업데이트
    }

    public void UpdateConditions()
    {
        // 체력
        // 체력이 최대가 아니고, 배고픔과 목마름이 회복 조건을 만족하며, 체온이 정상 범위에 있을 때 체력 회복
        if (health < maxHealth && hunger > hungerToHeal && Thirst > thirstToHeal && (bodyTemp > minNormalBodyTemp && bodyTemp < maxNormalBodyTemp))
        {
            GenerateHealth(healthRegenRate);
        }
        // 배고픔과 목마름이 회복 조건을 만족하지 않거나, 체온이 정상 범위를 벗어났을 때 체력 회복 안함
        else if ( hunger <= minHunger || thirst == minThirst || (bodyTemp == minNormalBodyTemp || bodyTemp == maxNormalBodyTemp))
        {
            GenerateHealth(-healthDegenRate * Time.deltaTime);
        }

        // 스태미나
        // 스태미나가 최대가 아니고, 배고픔과 목마름이 회복 조건을 만족하며, 플레이어가 달리지 않을 때 스태미나 회복
        if (stamina < maxStamina && hunger > minHunger && thirst > minThirst && !PlayerManager.Instance.controller.isRun)
        {
            GenerateStamina(staminaRegenRate);
        }
        // 플레이어가 달릴 때 스태미나 감소
        else if (PlayerManager.Instance.controller.isRun)
        {
            GenerateStamina(-staminaDegenRate);
        }




    }

    //체력 증가와 감소 메소드
    public void GenerateHealth(float _rate)
    {
        health += _rate * Time.deltaTime;
        health = Mathf.Clamp(health, minHealth, maxHealth);
    }

    //스태미나 증가와 감소 메소드
    public void GenerateStamina(float _amount)
    {
        stamina += _amount * Time.deltaTime;
        stamina = Mathf.Clamp(stamina, minStamina, maxStamina);
    }



    public void Heal(int _healAmount)
    {
        if(health < maxHealth)
        {
            health += _healAmount;
        }
        else if (health == maxHealth)
        {
            //체력 회복이 안되게 처리
            Debug.Log("Player health is full");
        }
    }

    public void TakePhysicalDamage(int _damageAmount)
    {
        if (health > minHealth)
        {
            health -= _damageAmount;
        }
        else if (health == minHealth)
        {
            //체력이 0이 되면 죽음 처리
            Debug.Log("Player is dead");
        }
    }
}
