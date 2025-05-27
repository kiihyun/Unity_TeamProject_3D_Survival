using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class PlayerCondition : MonoBehaviour, IDamagable
{
    [Header("Condition State")]
    public List<PlayerConditionState> conditionStats = new List<PlayerConditionState>(); //플레이어 상태 리스트

    [Header("Health")]
    [SerializeField] public float health;
    public float maxHealth; //최대 체력
    public float healthRecovRate; //체력 회복 속도
    public float healthDecRate; //체력 감소 속도
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
    public float maxStamina; //최대 스태미나
    public float staminaRecovRate = 0.1f; //스태미나 회복 속도
    public float staminaDecRate = 0.1f; //스태미나 감소 속도
    public float jumpDecStamina; //스태미나 회복 조건
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
    public float maxHunger;//최대 배고픔
    public float hungerDecRate = 3f;
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
    public float maxThirst; //최대 목마름
    public float thirstDegenRate = 3f; //목마름 감소 속도
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

    public PlayerController controller;

    //테스트용 UI 요소들
    public Slider healthUI;
    public Slider staminaUI;
    public Slider hungerUI;
    public Slider thirstUI;
    public TextMeshProUGUI tempUI;
    //-------------------------

    private void Awake()
    {
        if (!TryGetComponent<PlayerController>(out controller))
        {
            Debug.LogError("PlayerCondition is null");
        }
    }

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
        ConditionState(); //플레이어 상태 변경 메소드 호출
        TestUI(); //UI 업데이트 메소드 호출 (테스트용)
    }

    //플레이어 컨디션 상태 변경
    public void ConditionState()
    {
        // 배고픔 상태
        if (hunger <= 0f)
        {
            if (!conditionStats.Contains(PlayerConditionState.Hungry))
            {
                conditionStats.Add(PlayerConditionState.Hungry);
            }
        }
        else
        {
            conditionStats.Remove(PlayerConditionState.Hungry);
        }

        // 목마름 상태
        if (thirst <= 0f)
        {
            if (!conditionStats.Contains(PlayerConditionState.Thirsty))
            {
                conditionStats.Add(PlayerConditionState.Thirsty);
            }
        }
        else
        {
            conditionStats.Remove(PlayerConditionState.Thirsty);
        }

        // 체온 상태
        if (bodyTemp < minNormalBodyTemp)
        {
            if (!conditionStats.Contains(PlayerConditionState.Cold))
            {
                conditionStats.Add(PlayerConditionState.Cold);
            }
            conditionStats.Remove(PlayerConditionState.Fever);
        }
        else if (bodyTemp > maxNormalBodyTemp)
        {
            if (!conditionStats.Contains(PlayerConditionState.Fever))
            {
                conditionStats.Add(PlayerConditionState.Fever);
            }
            conditionStats.Remove(PlayerConditionState.Cold);
        }
        else
        {
            conditionStats.Remove(PlayerConditionState.Cold);
            conditionStats.Remove(PlayerConditionState.Fever);
        }
    }

    public void TestUI()
    {
        healthUI.value = health / maxHealth; //체력 UI 업데이트
        staminaUI.value = stamina / maxStamina; //스태미나 UI 업데이트
        hungerUI.value = hunger / maxHunger; //배고픔 UI 업데이트
        thirstUI.value = thirst / maxThirst; //목마름 UI 업데이트
        tempUI.text = $"Temp: {bodyTemp:F1}°C"; //체온 UI 업데이트
    }

    //회복 조건
    public void UpdateConditions()
    {
        // 체력
        // 체력이 최대가 아니고, 어떠한 이상상태가 없을 시 체력회복
        if (health < maxHealth
            && !conditionStats.Contains(PlayerConditionState.Hungry)
            && !conditionStats.Contains(PlayerConditionState.Thirsty)
            && !conditionStats.Contains(PlayerConditionState.Cold)
            && !conditionStats.Contains(PlayerConditionState.Fever)
            && hunger >= hungerToHeal
            && thirst >= thirstToHeal)
        {
            GenerateHealth(healthRecovRate);
        }
        // 어떠한 이상상태가 있을 시 체력감소
        else if (conditionStats.Contains(PlayerConditionState.Hungry)
            || conditionStats.Contains(PlayerConditionState.Thirsty)
            || conditionStats.Contains(PlayerConditionState.Cold)
            || conditionStats.Contains(PlayerConditionState.Fever))
        {
            GenerateHealth(-healthDecRate);
        }

        // 스태미나
        // 플레이어가 달리지 않고, 스태미나가 최대가 아니며, 배고픔과 목마름 상태가 없을 때 스태미나 회복
        if (stamina < maxStamina 
            && !conditionStats.Contains(PlayerConditionState.Hungry)
            && !conditionStats.Contains(PlayerConditionState.Thirsty)
            && controller.state != PlayerState.Run)
        {
            GenerateStamina(staminaRecovRate);
        }
        // 플레이어가 달릴 때 스태미나 감소
        else if (stamina > 0f && controller.state == PlayerState.Run)
        {
            GenerateStamina(-staminaDecRate);
        }

        //배고픔 지속적으로 줄어듬
        if(hunger > 0f)
        {
            GenerateHunger(hungerDecRate);
        }
        
        // 목마름 지속적으로 줄어듬
        if(thirst > 0f)
        {
            GenerateThirst(thirstDegenRate);
        }

    }

    //체력 증가와 감소 메소드
    public void GenerateHealth(float _amount)
    {
        health += _amount * Time.deltaTime;
    }

    //스태미나 증가와 감소 메소드
    public void GenerateStamina(float _amount)
    {
        stamina += _amount * Time.deltaTime;
    }

    //점프 시 스태미나 감소 메소드
    public void JumpStamina()
    {
        stamina -= jumpDecStamina;
    }

    //배고픔 증가 메소드
    public void GenerateHunger(float _amount)
    {
        hunger -= _amount * Time.deltaTime;
    }

    //음식 섭취 시, 배고픔 회복 메소드
    public void RecoverHunger(float _amount)
    {
        if (hunger < maxHunger)
        {
            hunger += _amount;
        }
        else
        {
            Console.WriteLine("Player hunger is full");
        }
    }

    //목마름 증가 메소드
    public void GenerateThirst(float _amount)
    {
        thirst -= _amount * Time.deltaTime;
    }

    //음식 섭취 시, 목마름 회복 메소드
    public void RecoverThirst(float _amount)
    {
        if (thirst < maxThirst)
        {
            thirst += _amount;
        }
        else
        {
            Console.WriteLine("Player thirst is full");
        }
    }


    //체력 회복 메소드
    public void Heal(int _healAmount)
    {
        if (health < maxHealth)
        {
            health += _healAmount;
        }
        else if (health >= maxHealth)
        {
            //체력 회복이 안되게 처리
            Debug.Log("Player health is full");
        }
    }

    //공격받는 메소드
    public void TakePhysicalDamage(int _damageAmount)
    {
        if (health > 0f)
        {
            health -= _damageAmount;
        }
        else if (health <= 0f)
        {
            //체력이 0이 되면 죽음 처리
            Debug.Log("Player is dead");
        }
    }
}
