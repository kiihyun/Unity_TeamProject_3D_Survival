using UnityEngine;

public enum PlayerState
{
    Idle,       //대기 상태
    Walk,       //걷기 상태
    Run,        //달리기 상태
    Jump,       //점프 상태
    Attack,     //공격 상태
    Dead        //죽음 상태
}

public enum PlayerConditionState
{
    Hungry,       //부상 상태
    Thirsty,      //배고픔 상태
    Cold,      //추위 상태
    Fever,      //열 상태
}