using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingUIManager : MonoBehaviour
{
    [Header("UI 연결")]
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI weightText;
    public TextMeshProUGUI verifyingSuccessText;  // 여기에 제작 성공/실패 메시지 출력
    public Button craftButton;

    [Header("재료 목록 UI")]
    public Transform requiredInfoParent;
    public GameObject requiredInfoPrefab;

    [Header("기본 연결")]
    public Inventory playerInventory;

    private CraftItemData currentRecipe;
    private bool isCrafting = false;
    private Button lastSelectedButton;

    public void DisplayRecipe(CraftItemData recipe)
    {
        currentRecipe = recipe;

        // 기본 정보 표시
        itemNameText.text = recipe.resultItem.displayName;
        descriptionText.text = recipe.resultItem.description;
        weightText.text = recipe.resultItem.itemMass + " kg";
        

        // 기존 재료 UI 지우기
        foreach (Transform child in requiredInfoParent)
        {
            Destroy(child.gameObject);
        }

        bool canCraft = true;

        foreach (var req in recipe.materials.Take(4))
        {
            int owned = playerInventory.GetItemCount(req.item);
            Debug.Log($"{req.item.displayName}: 가진 것 {owned} / 필요한 것 {req.amount}");
            if (owned < req.amount) canCraft = false;

            GameObject go = Instantiate(requiredInfoPrefab, requiredInfoParent);
            Debug.Log("재료 UI 생성됨: " + req.item.displayName);
            go.GetComponent<RequiredInfoUI>().Set(req.item.displayName, owned, req.amount);
            var ui = go.GetComponent<RequiredInfoUI>();
            if (ui == null)
            {
                Debug.LogError(" RequiredInfoUI 컴포넌트 없음!");
            }
            else
            {
                ui.Set(req.item.displayName, owned, req.amount);
                Debug.Log(" Set() 호출 성공");
            }
        }
        Debug.Log($"최종 제작 가능 여부: {canCraft}");
        craftButton.interactable = canCraft;
        // DisplayRecipe 끝나기 전에 강제로 보이게 하기
        foreach (Transform child in requiredInfoParent)
        {
            child.gameObject.SetActive(true);
            var texts = child.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var t in texts)
            {
                t.color = Color.white;
                t.fontSize = 36;
            }
        }

    }

    // 변경된 부분만 발췌
    public void OnClickCraft()
    {
        if (!craftButton.interactable || isCrafting) return;

        TryCraftItem();
    }

    void TryCraftItem()
    {
        bool hasAllMaterials = true;

        foreach (var req in currentRecipe.materials)
        {
            if (!playerInventory.HasItem(req.item, req.amount))
            {
                hasAllMaterials = false;
                break;
            }
        }

        if (hasAllMaterials)
        {
            foreach (var req in currentRecipe.materials)
            {
                playerInventory.RemoveItem(req.item, req.amount);
            }

            bool added = playerInventory.AddItem(currentRecipe.resultItem, currentRecipe.resultAmount);
            if (added)
            {
                verifyingSuccessText.text = "제작 성공!";
            }
            else
            {
                verifyingSuccessText.text = "제작 실패 (무게 초과 또는 슬롯 초과)";
            }

        }
        else
        {
            verifyingSuccessText.text = "제작 실패";
        }

        DisplayRecipe(currentRecipe); // UI 갱신
    }
    public void SelectSlot(Button newButton)
    {
        if (lastSelectedButton != null)
            lastSelectedButton.interactable = true; // 이전 버튼 다시 활성화

        lastSelectedButton = newButton; // 새 버튼 저장
        newButton.interactable = false; // 새 버튼 비활성화
    }

    void OnDisable()
    {
        // UI 초기화
        itemNameText.text = "";
        descriptionText.text = "";
        weightText.text = "";
        verifyingSuccessText.text = "";

        foreach (Transform child in requiredInfoParent)
        {
            Destroy(child.gameObject);
        }

        craftButton.interactable = false;

        // 버튼 선택 초기화
        if (lastSelectedButton != null)
        {
            lastSelectedButton.interactable = true;
            lastSelectedButton = null;
        }

        currentRecipe = null;
    }


}
