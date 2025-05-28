using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    //플레이어가 아이템이랑 상호작용해서 획득할 경우 정보를 넘겨주기 위해서 있는 클래스입니다.
    public ItemData itemData;
    public int amount = 1;//만약에 하나의 오브젝트를 얻어도 여러 개 채워지는 거라면 수량 변경도 가능하게

    // 플레이어가 호출함
    public void ItemInteract(Inventory inventory)
    {

        inventory.AddItem(itemData, amount);//플레이어의 인벤토리에 아이템 데이터와 수량을 넘겨줌
        Debug.Log($"{itemData.displayName} 얻음");//작동하는지 보기 위해서 여기에도 로그로 확인
        Destroy(gameObject); //아이템 얻었으니 파괴해줌
    }
}
