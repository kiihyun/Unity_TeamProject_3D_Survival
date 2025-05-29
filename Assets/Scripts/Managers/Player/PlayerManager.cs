using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public GameObject player;

    public PlayerController controller { get; private set; }
    public PlayerAnimController animator { get; private set; }
    public PlayerCondition condition { get; private set; }
    public PlayerInteraction interaction { get; private set; }
    public FootStep footStep { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Debug.Log("[PlayerManager] Awake 실행됨");

        player = gameObject;

        // 필수 컴포넌트 가져오기
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        interaction = GetComponent<PlayerInteraction>();

        // 자식 오브젝트에서 가져오기
        footStep = GetComponentInChildren<FootStep>();
        animator = GetComponentInChildren<PlayerAnimController>();

        // 예외 상황을 로그로 확인
        if (controller == null) Debug.LogError("PlayerController가 Player에 없습니다.");
        if (condition == null) Debug.LogError("PlayerCondition이 Player에 없습니다.");
        if (interaction == null) Debug.LogError("PlayerInteraction이 Player에 없습니다.");
        if (footStep == null) Debug.LogWarning("FootStep이 자식에 없습니다.");
        if (animator == null) Debug.LogWarning("PlayerAnimController가 자식에 없습니다.");
    }
}

